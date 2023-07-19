using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
  public class AllergicsPlanDto
  {
    public string Name { get; set; }
    public int? PlanId { get; set; }
  }
}
