using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
  public class GetDoctorNoteData
  {
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Text { get; set; }
  }
}
