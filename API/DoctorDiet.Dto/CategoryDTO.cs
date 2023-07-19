using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
    public class CategoryDTO
    {
        public string Name { get; set; }
        public List<MealDTO> Meal { get; set; }
    }
}
