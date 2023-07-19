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
    public enum Status
    {
        Waiting,
        Confirmed,
        Rejected,
        Cancled,
        Done
    }
    public class DoctorPatientBridge : IBaseModel<int>
    {
        public int Id { get; set; }
        [ForeignKey("Patient")]
        public string PatientID { get; set; }

        [ForeignKey("Doctor")]
        [DefaultValue("NoDoctor")]
        public string DoctorID { get; set; }
        public Status Status { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
    }
}
