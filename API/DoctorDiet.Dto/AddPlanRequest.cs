using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
  public class AddPlanRequest
  {
    public AddPlanDTO Plan { get; set; }
    public List<IFormFile> Images { get; set; }
    
  }
}
