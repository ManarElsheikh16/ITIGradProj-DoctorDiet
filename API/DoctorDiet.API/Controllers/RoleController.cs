using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Sakiny.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> Addrole(string role)
        {
            IdentityRole roleModel = new IdentityRole();
            roleModel.Name = role;
            
            IdentityResult result = await roleManager.CreateAsync(roleModel);


            if (result.Succeeded)
            {
                return Ok("Added");
            }
            else
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

        }
    }
}
