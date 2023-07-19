using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Models
{
    public class ApplicationUser : IdentityUser
    {
        public byte[]? ProfileImage { get; set; }


        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
    }
}
