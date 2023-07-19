using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DoctorDiet.DTO
{
    public class RegisterAdminDto
    {
        public string FullName { get; set; }

        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
         
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public IFormFile? ProfileImage { get; set; }

    }
}