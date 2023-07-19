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
      public enum Goal
      {
        weightLoss,
        weightGain,
        muscleBuilding
      }
      public enum Gender
      {
        Male,
        Female
      }
  public class Plan:IBaseModel<int>
    {
        public int Id { get; set; }
        public int CaloriesFrom { get; set; }
        public int CaloriesTo { get; set; }
        public int Duration { get; set; }
        public virtual List<Day> Days { get; set; }
        public virtual IEnumerable<AllergicsPlan>? Allergics { get; set; }

        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
        [ForeignKey("Doctor")]
        public string DoctorID { get; set; }

       public Goal goal { get; set; }
       public Gender gender { get; set; }

       public virtual Doctor Doctor { get; set; }
    }
}
