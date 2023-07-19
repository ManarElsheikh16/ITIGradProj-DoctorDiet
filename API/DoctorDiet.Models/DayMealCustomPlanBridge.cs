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
  public class DayMealCustomPlanBridge:IBaseModel<int>
  {
    public DayMealCustomPlanBridge() { }
    public int Id { get; set; }
    [ForeignKey("DayCustomPlan")]
    public int DayId { get; set; }
    [ForeignKey("MealCustomPlan")]
    public int MealId { get; set; }
    public virtual DayCustomPlan? DayCustomPlan { get; set; }
    public virtual MealCustomPlan? MealCustomPlan { get; set; }
    [DefaultValue("false")]
    public bool IsDeleted { get; set; }
  }
}
