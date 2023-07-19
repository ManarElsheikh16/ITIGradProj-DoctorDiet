using DoctorDiet.Dto;
using DoctorDiet.DTO;
using DoctorDiet.Models;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoctorDiet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {

        AdminService adminService;
        IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;


        public AdminController(AdminService adminService ,IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            this.adminService = adminService;
            this.unitOfWork = unitOfWork;
            _userManager = userManager;
        }


        [HttpGet("Adminid")]
        public IActionResult GetAdminById(string Adminid)
        {
            var Admin = adminService.GetAdminData(Adminid);

            return Ok(Admin);

        }



        [HttpPut("Adminid")]
        public IActionResult EditAdminData(  [FromForm]  AdminDataDTO adminDataDTO,[FromForm] params string[] properties)
        {
            if(ModelState.IsValid) {
                

                adminService.EditAdminData(adminDataDTO, properties);
              
                unitOfWork.CommitChanges();
                return Ok("sucess");
            }


            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPost("ChangePassowrd")]
    public async Task<IActionResult> ChangePassowrd(AdminNewPasswordDTO newPassowrd)
    {

      if (ModelState.IsValid)
      {
        Admin admin = adminService.GetAdminData(newPassowrd.AdminId);

        if (admin != null)
        {
          PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
          var newPasswordHash = passwordHasher.HashPassword(admin.ApplicationUser, newPassowrd.Password);

          admin.ApplicationUser.PasswordHash = newPasswordHash;
          var result = await _userManager.UpdateAsync(admin.ApplicationUser);
          RegisterDto registerDto = new RegisterDto();
          unitOfWork.CommitChanges();
          if (result.Succeeded)
          {
            registerDto.Message = "Success";
            return Ok(registerDto);
          }
          else
            registerDto.Message = "Failed";
          return BadRequest(registerDto);
        }

      }
      return BadRequest(ModelState);

    }

  }
}
