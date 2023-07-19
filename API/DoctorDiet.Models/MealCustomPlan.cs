using DoctorDiet.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Models
{
  public enum MealCategory
  {
    Breakfast,
    Lanuch,
    Dinner,
    Sohour,
    Snacks,
    other
  }
    
    public class MealCustomPlan : IBaseModel<int>
  {

    public int Id { get; set; }
    public string Description { get; set; }
    public IEnumerable<DayMealCustomPlanBridge>? DayMealCustomPlanBridge { get; set; }
    public MealCategory Category { get; set; }
     public Type Type { get; set; }
     public byte[]? Image { get; set; }

    [DefaultValue("false")]
    public bool IsDeleted { get; set; }
  }
}

