using App.Services.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        [HttpGet]
        [Route("get-public")]
        public IActionResult GetPublicData()
        {
            return Ok("Public Data");
        }

        [HttpGet]
        [Route("get-user-role")]
        [Authorize(Roles = StaticUserRoles.AppUser)]
        public IActionResult GetUserData()
        {
            return Ok("User Role Data");
        }

        [HttpGet]
        [Route("get-manager-role")]
        [Authorize(Roles = StaticUserRoles.Manager)]
        public IActionResult GetManagerData()
        {
            return Ok("Manager Role Data");
        }

        [HttpGet]
        [Route("get-admin-role")]
        [Authorize(Roles = StaticUserRoles.Admin)]
        public IActionResult GetAdminData()
        {
            return Ok("Admin Role Data");
        }

        [HttpGet]
        [Route("get-superadmin-role")]
        [Authorize(Roles = StaticUserRoles.SuperAdmin)]
        public IActionResult GetOwnerData()
        {
            return Ok("SuperAdmin Role Data");
        }
    }
}
