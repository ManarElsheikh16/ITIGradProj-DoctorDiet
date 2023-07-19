using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
  public class ContactInfoDTO
  {
    public int? Id { get; set; } 
    public string contactInfo { get; set; }
    public string DoctorId { get; set; }

  }
}
