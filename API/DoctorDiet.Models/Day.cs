using DoctorDiet.Models.Interface;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoctorDiet.Models
{
   
    public class Day:IBaseModel<int>  {
        [Key]
        public int Id { get; set; }
        public IEnumerable<DayMealBridge>? DayMeal { get; set; }
        [DefaultValue("false")]
        public bool IsDeleted { get; set; }
        [ForeignKey("Plan")]
        public int PlanId { get; set; }
        public Plan Plan { get; set; }
    }
}
