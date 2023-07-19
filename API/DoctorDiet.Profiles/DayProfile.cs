using AutoMapper;
using DoctorDiet.Dto;
using DoctorDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Profiles
{
  public class DayProfile:Profile
  {
    public DayProfile()
    {
      CreateMap<DayDTO, Day>();

    }
  }
}
