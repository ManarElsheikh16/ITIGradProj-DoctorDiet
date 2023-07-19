
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
    public class PlanDataDTO
    {
        public int Id { get; set; }
        public int CaloriesFrom { get; set; }
        public int CaloriesTo { get; set; }
        public int Duration { get; set; }
        public string DoctorID { get; set; }
        public string goal { get; set; }
        public string gender { get; set; }
    }
}
