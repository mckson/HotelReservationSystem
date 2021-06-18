using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Constants;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Authorize(Policy = Policies.AdminPermission)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;

        public UsersController(
            IUsersService usersService,
            IMapper mapper)
        {
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> GetAllUsersAsync()
        {
            var userModels = await _usersService.GetAllUsersAsync();
            var userResponseModels =
                _mapper.Map<IEnumerable<UserResponseModel>>(userModels);

            return Ok(userResponseModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseModel>> GetUserByIdAsync(string id)
        {
            var userModel = await _usersService.GetAsync(id);

            return Ok(_mapper.Map<UserResponseModel>(userModel));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseModel>> CreateUserAsync([FromBody] UserAdminCreationRequestModel creatingUser)
        {
            var creatingUserModel = _mapper.Map<UserRegistrationModel>(creatingUser);
            var addedUser = await _usersService.CreateAsync(creatingUserModel);

            return Ok(_mapper.Map<UserResponseModel>(addedUser));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseModel>> UpdateUserAsync(string id, [FromBody] UserUpdateRequestModel user)
        {
            var currentUserClaims = User.Claims;

            var userUpdateModel = _mapper.Map<UserUpdateModel>(user);
            var updatedUserModel = await _usersService.UpdateAsync(id, userUpdateModel, currentUserClaims);
            var updatedUserResponseModel = _mapper.Map<UserResponseModel>(updatedUserModel);

            return Ok(updatedUserResponseModel);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserResponseModel>> DeleteUserByIdAsync(string id)
        {
            var currentUserClaims = User.Claims;
            var deletedUser = await _usersService.DeleteAsync(id, currentUserClaims);
            var deletedUserResponse = _mapper.Map<UserResponseModel>(deletedUser);

            return Ok(deletedUserResponse);
        }
    }
}
