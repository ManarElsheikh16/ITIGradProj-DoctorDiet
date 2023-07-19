using AutoMapper;
using DoctorDiet.Dto;
using DoctorDiet.DTO;
using DoctorDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakiny.Profiles
{
    public class RegisterAdminProfile:Profile
    {
        public RegisterAdminProfile()
        {
            CreateMap<RegisterAdminDto, ApplicationUser>()
            .ForMember(dst => dst.ProfileImage, opt =>opt.Ignore());

            CreateMap<AdminDataDTO, Admin>()
                .ForPath(dst => dst.ApplicationUser.UserName,
                opt => opt.MapFrom(src => src.UserName))

                .ForPath(dst => dst.ApplicationUser.Email,
                opt => opt.MapFrom(src => src.Email))

                .ForPath(dst => dst.ApplicationUser.ProfileImage,
                opt => opt.MapFrom(src => src.ProfileImage))
               .ForPath(dst => dst.ApplicationUser.ProfileImage, opt => opt.Ignore());

             CreateMap<RegisterAdminDto, Admin>();



        }
    }
}
