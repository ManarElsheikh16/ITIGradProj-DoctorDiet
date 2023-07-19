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
  public class DayCustomPlan : IBaseModel<int>
  {
    public int Id { get; set; }
    public IEnumerable<DayMealCustomPlanBridge> DayMealCustomPlanBridge { get; set; }
    [ForeignKey("CustomPlan")]
    public int CustomPlanId { get; set; }
    public CustomPlan CustomPlan { get; set; }

    public IEnumerable<PatientNotes> PatientNotes { get; set; }
    [DefaultValue("false")]
    public bool IsDeleted { get; set; }
  }
}
