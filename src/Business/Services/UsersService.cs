using AutoMapper;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models;
using HotelReservation.Business.Models.UserModels;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservation.Business.Services
{
    public class UsersService : IUsersService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<UserEntity> _passwordHasher;
        private readonly IHotelRepository _hotelRepo;
        private readonly ILogger _logger;
        private readonly IReservationsService _reservationService;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<UserEntity> _userManager;

        public UsersService(
            UserManager<UserEntity> userManager,
            IUserRepository userRepository,
            IPasswordHasher<UserEntity> passwordHasher,
            IHotelRepository hotelRepo,
            IReservationsService reservationsService,
            IMapper mapper,
            ILogger logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _hotelRepo = hotelRepo;
            _reservationService = reservationsService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            _logger.Debug("Users are requesting");

            var userEntities = await _userRepository.GetAll();
            var userModels = _mapper.Map<IEnumerable<UserModel>>(userEntities);

            var users = userModels.ToList();
            foreach (var userModel in users)
            {
                // await GetRolesForUserModelAsync(userModel);
                GetReservationsForUser(userModel);
            }

            _logger.Debug("Users requested");

            return users;
        }

        public async Task<UserModel> CreateAsync(UserRegistrationModel userRegistration)
        {
            _logger.Debug($"User {userRegistration.FirstName} {userRegistration.LastName} ({userRegistration?.Email}) is creating");

            if (userRegistration == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var existingUserEntity = await _userRepository.GetByEmailAsync(userRegistration.Email);

            if (existingUserEntity != null)
                throw new BusinessException("User with such email already exists", ErrorStatus.AlreadyExist);

            var userEntity = _mapper.Map<UserEntity>(userRegistration);

            userEntity.UserName ??= userRegistration.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];
            userEntity.PasswordHash = _passwordHasher.HashPassword(userEntity, userRegistration.Password);

            var result = await _userRepository.CreateAsync(userEntity);
            if (result)
            {
                foreach (var role in userRegistration.Roles)
                {
                    var roleResult = await _userRepository.AddToRoleAsync(userEntity, role);

                    if (!roleResult)
                    {
                        await _userRepository.DeleteAsync(userEntity.Id);
                        throw new BusinessException(
                            $"Cannot add user to unknown role {role}",
                            ErrorStatus.IncorrectInput);
                    }
                }
            }
            else
            {
                throw new BusinessException(
                    "User with such nickname or email already exists",
                    ErrorStatus.IncorrectInput);
            }

            var addedUserEntity = _userRepository.GetByIdAsync(userEntity.Id); // to attract roles on repository level
            var addedUserModel = _mapper.Map<UserModel>(addedUserEntity);

            _logger.Debug($"User with id {userEntity.Id}, {userEntity.FirstName} {userEntity.LastName} ({userEntity.Email}) is created");

            return addedUserModel;
        }

        public async Task<UserModel> GetAsync(Guid id)
        {
            _logger.Debug($"User {id} is requesting");

            var userEntity = await _userRepository.GetByIdAsync(id) ??
                             throw new BusinessException($"User with id {id} does not exist", ErrorStatus.NotFound);
            var userModel = _mapper.Map<UserModel>(userEntity);
            // await GetRolesForUserModelAsync(userModel);
            GetReservationsForUser(userModel);

            _logger.Debug($"User {id} requested");

            return userModel;
        }

        public async Task<bool> DeleteAsync(Guid id, IEnumerable<Claim> currentUserClaims)
        {
            _logger.Debug($"User {id} is deleting");

            if (currentUserClaims.FirstOrDefault(cl => cl.Type == "id")?.Value == id.ToString())
            {
                throw new BusinessException("You cannot delete yourself", ErrorStatus.IncorrectInput);
            }

            // await GetRolesForUserModelAsync(deletedUserModel);
            var result = await _userRepository.DeleteAsync(id);

            if (!result)
            {
                throw new BusinessException("No user with such id", ErrorStatus.NotFound);
            }

            _logger.Debug($"User {id} is deleted");

            return true;
        }

        public async Task<UserModel> UpdateAsync(Guid id, UserUpdateModel updatingUserUpdateModel, IEnumerable<Claim> currentUserClaims)
        {
            _logger.Debug($"User {id} is updating");

            if (updatingUserUpdateModel == null)
                throw new BusinessException("User cannot be empty", ErrorStatus.EmptyInput);

            var userEntity = await _userRepository.GetByIdAsync(id);

            if (updatingUserUpdateModel.Email != null)
                userEntity.Email = updatingUserUpdateModel.Email;

            if (updatingUserUpdateModel.PhoneNumber != null)
                userEntity.PhoneNumber = updatingUserUpdateModel.PhoneNumber;

            if (updatingUserUpdateModel.DateOfBirth != null)
                userEntity.DateOfBirth = updatingUserUpdateModel.DateOfBirth.Value;

            if (updatingUserUpdateModel.FirstName != null)
                userEntity.FirstName = updatingUserUpdateModel.FirstName;

            if (updatingUserUpdateModel.LastName != null)
                userEntity.LastName = updatingUserUpdateModel.LastName;

            if (updatingUserUpdateModel.UserName != null)
                userEntity.UserName = updatingUserUpdateModel.UserName;

            var hotelUsers = new List<HotelUserEntity>();
            userEntity.HotelUsers.RemoveAll(hu => hu.UserId == userEntity.Id);

            if (updatingUserUpdateModel.Hotels != null)
            {
                // userEntity.HotelUsers = new List<HotelUserEntity>();
                foreach (var hotel in updatingUserUpdateModel.Hotels)
                {
                    // was as no tracking
                    var unused = await _hotelRepo.GetAsync(hotel) ??
                                 throw new BusinessException("There is no hotel with such id", ErrorStatus.NotFound);

                    // userEntity.HotelUsers.Add(new HotelUserEntity() { HotelId = hotel });
                    hotelUsers.Add(new HotelUserEntity() { HotelId = hotel });
                }
            }

            if (updatingUserUpdateModel.Email != null)
                userEntity.UserName ??= updatingUserUpdateModel.Email.Split('@', StringSplitOptions.RemoveEmptyEntries)[0];

            if (updatingUserUpdateModel.NewPassword != null)
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(
                    userEntity,
                    updatingUserUpdateModel.OldPassword,
                    updatingUserUpdateModel.NewPassword);

                if (changePasswordResult != IdentityResult.Success)
                {
                    var sb = new StringBuilder();
                    foreach (var error in changePasswordResult.Errors)
                    {
                        sb.Append(error.Description + ' ');
                    }

                    throw new BusinessException(sb.ToString(), ErrorStatus.IncorrectInput, changePasswordResult.Errors);
                }
            }

            // var result = await _userManager.UpdateAsync(userEntity);
            if (hotelUsers.Count > 0)
            {
                userEntity.HotelUsers = hotelUsers;
            }

            var result = await _userRepository.UpdateAsync(userEntity);

            if (result)
            {
                if (updatingUserUpdateModel.Roles != null)
                {
                    var addedUserRoles = userEntity.Roles;
                    var userId = Guid.Parse(currentUserClaims.FirstOrDefault(cl => cl.Type == ClaimNames.Id).Value);

                    if (!addedUserRoles.Contains(Roles.User))
                        await _userRepository.AddToRoleAsync(userEntity, Roles.User);

                    foreach (var role in addedUserRoles)
                    {
                        if (!updatingUserUpdateModel.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
                        {
                            if (string.Equals(role, Roles.Admin, StringComparison.InvariantCultureIgnoreCase) &&
                                userEntity.Id.Equals(userId))
                            {
                                throw new BusinessException(
                                    "You cannot change your own admin role",
                                    ErrorStatus.IncorrectInput);
                            }

                            if (string.Equals(role, Roles.User, StringComparison.InvariantCultureIgnoreCase))
                            {
                                continue;
                            }

                            await _userManager.RemoveFromRoleAsync(userEntity, role.ToUpper());
                        }
                    }

                    foreach (var role in updatingUserUpdateModel.Roles)
                    {
                        if (!addedUserRoles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
                        {
                            var roleResult = await _userRepository.AddToRoleAsync(userEntity, role);
                            if (!roleResult)
                            {
                                throw new BusinessException(
                                    $"Cannot add user to role {role}, because it is already in role or it is nonexistent role",
                                    ErrorStatus.IncorrectInput);
                            }
                        }
                    }
                }
            }

            var updatedUserEntity = await _userRepository.GetByIdAsync(userEntity.Id);
            var addedUserModel = _mapper.Map<UserModel>(updatedUserEntity);

            _logger.Debug($"User {id} updated");

            return addedUserModel;
        }

        /*
        private async Task GetRolesForUserModelAsync(UserModel userModel)
        {
            _logger.Debug($"User {userModel.Id} roles are requesting");

            var roles = await _userManager.GetRolesAsync(_mapper.Map<UserEntity>(userModel));
            userModel.Roles = roles;

            _logger.Debug($"User {userModel.Id} roles requested");
        }
        */
        private void GetReservationsForUser(UserModel userModel)
        {
            var reservations = _reservationService.GetReservationsByEmail(userModel.Email);
            userModel.Reservations = reservations;
        }
    }
}
