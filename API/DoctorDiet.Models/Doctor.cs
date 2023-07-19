using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorDiet.Models.Interface;

namespace DoctorDiet.Models
{
    public class Doctor:IBaseModel<string>
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public string FullName { get; set; }

        [DefaultValue("false")]
        public bool IsDeleted { get; set; }

        public IEnumerable<ContactInfo> ContactInfo { get; set; }
        public IEnumerable<PatientNotes>? PatientNotes { get; set; }

    public virtual IEnumerable<Plan>? Plan { get; set; }

      public virtual IEnumerable<DoctorPatientBridge>? DoctorPatientBridge { get; set; }

        public string Specialization { get; set; }
        public string Location { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }


    }
}
