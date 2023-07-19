using DoctorDiet.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = DoctorDiet.Models.Type;

namespace DoctorDiet.Dto
{
    public class CustomMealsDTO
    {
        public byte[] Image { get; set; }
        public int Id { get; set; }
        public int CustomPlanId { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }

        public Type Type { get; set; }
    }
}
