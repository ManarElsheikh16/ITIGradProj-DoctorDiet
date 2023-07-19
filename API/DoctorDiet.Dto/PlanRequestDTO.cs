using DoctorDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
    public class PlanRequestDTO
    {
        public CustomPlan Plan { get; set; }
        public Patient CurrentPatient { get; set; }
    }
}
