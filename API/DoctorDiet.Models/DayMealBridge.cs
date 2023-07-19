using DoctorDiet.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Models
{
  public class DayMealBridge:IBaseModel<int>
  {
    public DayMealBridge() { }
    public int Id { get; set; }
    [ForeignKey("Day")]
    public int DayId { get; set; }
    [ForeignKey("Meal")]
    public int MealId { get; set; }
    public virtual Day Day { get; set; }
    public virtual Meal Meal { get; set; }

    [DefaultValue("false")]
    public bool IsDeleted { get; set; }
  }
}
