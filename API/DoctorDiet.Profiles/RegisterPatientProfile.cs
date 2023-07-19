using AutoMapper;
using DoctorDiet.Dto;
using DoctorDiet.DTO;
using DoctorDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Profiles
{
    public class RegisterPatientProfile:Profile
    {
        public RegisterPatientProfile()
        {
            CreateMap<RegisterPatientDto, ApplicationUser>()
            .ForMember(dst => dst.ProfileImage, opt => opt.Ignore())
            .ForMember(dst => dst.PhoneNumber, opt => opt.MapFrom(src => src.phoneNumber));

            CreateMap<RegisterPatientDto, Patient>()
                .ForMember(dst => dst.NoEat, opt => opt.Ignore()).ReverseMap();

      CreateMap<DoctorPatientBridge, PatientDTO>()
                .ForMember(src => src.Id, opt => opt.MapFrom(dst => dst.Patient.Id))
               .ForMember(src => src.FullName, opt => opt.MapFrom(dst => dst.Patient.FullName))
               .ForMember(src => src.Email, opt => opt.MapFrom(dst => dst.Patient.ApplicationUser.Email))
               .ForMember(src => src.PhoneNumber, opt => opt.MapFrom(dst => dst.Patient.ApplicationUser.PhoneNumber))
               .ForMember(src => src.Weight, opt => opt.MapFrom(dst => dst.Patient.Weight))
               .ForMember(src => src.Height, opt => opt.MapFrom(dst => dst.Patient.Height))
               .ForMember(src => src.Goal, opt => opt.MapFrom(dst => dst.Patient.Goal))
               .ForMember(src => src.MinCalories, opt => opt.MapFrom(dst => dst.Patient.MinCalories))
               .ForMember(src => src.MaxCalories, opt => opt.MapFrom(dst => dst.Patient.MaxCalories))
               .ForMember(dst => dst.ProfileImage, opt =>
               opt.MapFrom(src => src.Patient.ApplicationUser.ProfileImage));

            CreateMap<EditPatientDto, Patient>()
                      .ForPath(dest => dest.ApplicationUser.Email, opt => opt.MapFrom(src => src.Email))

                      .ForPath(dst => dst.ApplicationUser.ProfileImage, opt => opt.Ignore())
                      .ForPath(dest => dest.ApplicationUser.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Patient, PatientDTO>()
                     .ForMember(src => src.Email, opt => opt.MapFrom(dst => dst.ApplicationUser.Email))
                     .ForMember(src => src.PhoneNumber, opt => opt.MapFrom(dst => dst.ApplicationUser.PhoneNumber))
                     .ForMember(dst => dst.ProfileImage, opt =>
                     opt.MapFrom(src => src.ApplicationUser.ProfileImage));

    }
    }
}
