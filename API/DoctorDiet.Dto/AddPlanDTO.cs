using DoctorDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Dto
{
    public class AddPlanDTO
    {
    public int CaloriesFrom { get; set; }
    public int CaloriesTo { get; set; }
    public int Duration { get; set; }
    public string DoctorID { get; set; }
    public Goal goal { get; set; }
    public Gender gender { get; set; }
    public List<AllergicsPlanDto>? Allergics { get; set; }
    public  List<DayDTO>? Days { get; set; }
    }
}
