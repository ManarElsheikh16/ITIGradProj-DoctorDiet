using DoctorDiet.Models.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Models
{
  public class AllergicsPlan : IBaseModel<int>
  {
    public int Id { get; set; }
    public string Name { get; set; }


    [DefaultValue("false")]
    public bool IsDeleted { get; set; }

    [ForeignKey("Plan")]
    public int PlanId { get; set; }
    public Plan Plan { get; set; }
  }
}
