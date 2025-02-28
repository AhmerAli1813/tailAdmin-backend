using App.Services.Dto.Account;
using App.Services.Dto.General;
using App.Services.Helper;
using App.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _authService;

        public AccountController(IAccountService authService)
        {
            _authService = authService;
        }


        // Route -> Register
        [HttpPost]
        [Route("register")]
        [Authorize(Roles = StaticUserRoles.CountryHeadAdminRole)]

        public async Task<IActionResult> Register([FromBody] AccountDto registerDto)
        {
            var registerResult = await _authService.RegisterAsync(registerDto);
            return StatusCode(registerResult.StatusCode, registerResult.Message);
        }
        // Route -> Register
        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> update([FromBody] AccountDto registerDto)
        {
            var registerResult = await _authService.UpdateAsync(registerDto);
            return StatusCode(registerResult.StatusCode, registerResult.Message);
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginServiceResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            LoginServiceResponseDto? loginResult = await _authService.LoginAsync(loginDto);

            if (loginResult is null || loginResult.ResponseCode == StatusCodes.Status401Unauthorized)
            {
                return Unauthorized("Your credentials are invalid. Please contact to an Admin");
            }
            if (loginResult.ResponseCode == StatusCodes.Status200OK)
            {

                return Ok(loginResult);
            }
            return StatusCode(loginResult.ResponseCode);
        }

        // Route -> Update User Role
        // An Owner can change everything
        // An Admin can change just User to Manager or reverse
        // Manager and User Roles don't have access to this Route
        [HttpPost]
        [Route("update-role")]
        [Authorize(Roles = StaticUserRoles.SuperAdmin_Admin_Manager)]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto updateRoleDto)
        {
            var updateRoleResult = await _authService.UpdateRoleAsync(User, updateRoleDto);

            if (updateRoleResult.IsSucceed)
            {
                return Ok(updateRoleResult.Message);
            }
            else
            {
                return StatusCode(updateRoleResult.StatusCode, updateRoleResult.Message);
            }
        }

        // Route -> getting data of a user from it's JWT
        [HttpPost]
        [Route("me")]
        [AllowAnonymous]

        public async Task<IActionResult> Me([FromBody] MeDto token)
        {
            try
            {
                LoginServiceResponseDto? me = await _authService.MeAsync(token);
                if (me is not null)
                {
                    return Ok(me);
                }
                else
                {
                    return StatusCode(me.ResponseCode);
                }
            }
            catch (Exception)
            {
                return Unauthorized("Invalid Token");
            }
        }

        // Route -> List of all users with details
        [HttpPost]
        [Route("users")]
        public async Task<ActionResult<IEnumerable<UserInfoResult>>> GetUsersList(UsersFilterDto filterDto)
        {
            try
            {

                var usersList = await _authService.GetUsersListAsync(filterDto);
                return Ok(usersList);
            }
            catch (Exception ex)
            {
                return BadRequest("Errro: " + ex.Message);
            }
        }


        // Route -> Get a User by UserName
        [HttpGet]
        [Route("users/{userName}")]
        public async Task<ActionResult<UserInfoResult>> GetUserDetailsByUserName([FromRoute] string userName)
        {
            var user = await _authService.GetUserDetailsByUserNameAsync(userName);
            if (user is not null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound("UserName not found");
            }
        }

        // Route -> Get List of all usernames for send message
        [HttpGet]
        [Route("usernames")]
        public async Task<ActionResult<IEnumerable<string>>> GetUserNamesList()
        {
            var usernames = await _authService.GetUsernamesListAsync();

            return Ok(usernames);
        }
        [HttpGet]
        [Route("get-Regional-RelationShip-user-list")]
        public async Task<IActionResult> GetRegionalAndRelationshipUsersAsync()
        {
         return   Ok(await _authService.GetAllRegionalAndRelationshipUsersAsync());
        }
        

        [HttpPost]
        [Route("lock-user")]
        [Authorize(Roles = StaticUserRoles.CountryHeadAdminRole)]
        public async Task<IActionResult> LockoutUser(UserStatusDto req)
        {
            ResponseDto a = await _authService.LockoutEnabled(req);
            return a.StatusCode == StatusCodes.Status200OK ? Ok(a) : StatusCode(a.StatusCode, a.Message);
        }
        [HttpPost]
        [Route("user-set-default-password")]
        [Authorize(Roles = StaticUserRoles.CountryHeadAdminRole)]
        public async Task<IActionResult> ResetPasswordSetDefault(ResetPasswordSetDefaultDto req)
        {
            ResponseDto a = await _authService.ResetPasswordSetDefault(req);
            return a.StatusCode == StatusCodes.Status200OK ? Ok(a) : StatusCode(a.StatusCode, a.Message);
        }
    }
}
