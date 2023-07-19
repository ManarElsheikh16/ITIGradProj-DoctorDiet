using DoctorDiet.Models.Interface;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorDiet.Models
{
    public class NoEat:IBaseModel<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }


        [DefaultValue("false")]
        public bool IsDeleted { get; set; }

        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
