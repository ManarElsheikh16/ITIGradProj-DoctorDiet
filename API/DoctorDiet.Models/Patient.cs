using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using DoctorDiet.Models.Interface;

namespace DoctorDiet.Models
{
   
    public class Patient : IBaseModel<string>
    {

        public Patient() { }

        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string FullName { get; set; }
        public Gender Gender { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public Goal Goal { get; set; }
        public DateTime BirthDate { get; set; }
        public string Diseases { get; set; }
        public int? MaxCalories { get; set; }
        public int? MinCalories { get; set; }
        public virtual IEnumerable<DoctorPatientBridge>? DoctorPatientBridge { get; set; }
        public virtual IEnumerable<NoEat> NoEat { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual List<CustomPlan> CustomPlans { get; set; }
        public virtual IEnumerable<DoctorNotes> DoctorNotes { get; set; }
        public virtual string ActivityRates { get; set; }
        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }


    }
}
