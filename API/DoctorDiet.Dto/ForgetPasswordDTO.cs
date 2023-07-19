using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
    public class ForgetPasswordDTO
    {
        public string UserName { get; set; }
        public string NewPass { get; set; }

        public string Answer { get; set; }
    }
}
