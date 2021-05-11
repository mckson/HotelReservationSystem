using AutoMapper;
using HotelReservation.API.Models.RequestModels;
using HotelReservation.API.Models.ResponseModels;
using HotelReservation.Business.Interfaces;
using HotelReservation.Business.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelReservation.API.Controllers
{
    [Authorize(Policy = "UserManagementPermission")]
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

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> GetAllUsersAsync()
        {
            try
            {
                var userModels = await _usersService.GetAllUsersAsync();
                var userResponseModels =
                    _mapper.Map<IEnumerable<UserResponseModel>>(userModels);
                return Ok(userResponseModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseModel>> GetUserBuId(string id)
        {
            var userModel = await _usersService.GetAsync(id);
            return Ok(_mapper.Map<UserResponseModel>(userModel));
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<UserResponseModel>> CreateUser([FromBody] UserAdminCreationRequestModel createdUser)
        {
            var createdUserModel = _mapper.Map<UserRegistrationModel>(createdUser);
            var addedUser = await _usersService.CreateAsync(createdUserModel);

            return Ok(_mapper.Map<UserResponseModel>(addedUser));
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseModel>> UpdateUserAsync(string id, [FromBody] UserUpdateRequestModel user)
        {
            var currentUserClaims = User.Claims;
            var userUpdateModel = _mapper.Map<UserUpdateModel>(user);
            var updatedUserModel = await _usersService.UpdateAsync(id, userUpdateModel, currentUserClaims);
            var updatedUserResponseModel = _mapper.Map<UserResponseModel>(updatedUserModel);

            return Ok(updatedUserResponseModel);
        }

        // DELETE api/<UsersController>/5
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
