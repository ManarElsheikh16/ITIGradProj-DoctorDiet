using DoctorDiet.Dto;
using DoctorDiet.Models;
using System.ComponentModel.DataAnnotations;

namespace DoctorDiet.DTO
{
    public class RegisterDoctorDto: RegisterAdminDto
    {
        public string Specialization { get; set; }
        public string Location { get; set; }

        public List<string> ContactInfo { get; set; }

        public string Question { get; set; }
        public string Answer { get; set; }


    }
}
