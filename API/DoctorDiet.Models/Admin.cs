using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoctorDiet.Models.Interface;

namespace DoctorDiet.Models
{
    public class Admin:IBaseModel<string>
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string FullName { get; set; }


        [DefaultValue("false")]
        public bool IsDeleted { get; set; }

    }
}
