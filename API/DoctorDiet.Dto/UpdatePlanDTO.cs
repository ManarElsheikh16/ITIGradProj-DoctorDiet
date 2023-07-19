using DoctorDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
    public class UpdatePlanDTO
    {
        public int Id { get; set; }
        public int CaloriesFrom { get; set; }
        public int CaloriesTo { get; set; }
        public int Duration { get; set; }
        public Goal goal { get; set; }
        public Gender gender { get; set; }
    }
}
