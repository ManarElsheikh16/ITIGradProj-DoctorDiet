using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.DTO
{
    public class RegisterDto
    {
        public bool? IsPass { get; set; }
        public dynamic? Data { get; set; }
        public string? Message { get; set; }
    }
}
