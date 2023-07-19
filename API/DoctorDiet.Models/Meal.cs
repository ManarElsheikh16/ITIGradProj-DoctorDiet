using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DoctorDiet.Models.Interface;
using Microsoft.AspNetCore.Http;

namespace DoctorDiet.Models
{
  public enum Category
  {
    Breakfast,
    Lanuch,
    Dinner,
    Sohour,
    Snacks,
    other
  }
  public enum Type
  {
    Basic,
    SubStitute
  }
  public class Meal:IBaseModel<int>
    {

        public int Id { get; set; }
        public string Description { get; set; }
         public IEnumerable<DayMealBridge>? DayMeal { get; set; }
         public Category Category { get; set; }
         public Type Type { get; set; }
        public byte[] Image { get; set; }

        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
    }
}
