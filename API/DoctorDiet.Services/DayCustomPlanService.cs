using AutoMapper;
using DoctorDiet.Dto;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using DoctorDiet.Repository.Repositories;
using DoctorDiet.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Services
{
  public class DayCustomPlanService
  {
    IMapper _mapper;
    IGenericRepository<CustomPlan, int> _CustomPlanRepository;

    public DayCustomPlanService(
      IMapper mapper, IGenericRepository<CustomPlan, int> customPlanRepository)
    {
      _mapper = mapper;
      _CustomPlanRepository = customPlanRepository;
    }

    public CustomDayDTO GetDayCustomPlan(int id)
    {
      CustomPlan myPlan = _CustomPlanRepository.Get(cusID => cusID.Id == id)
       .Include(d => d.DaysCustomPlan)
      .ThenInclude(bridge => bridge.DayMealCustomPlanBridge)
      .ThenInclude(m => m.MealCustomPlan)
      .FirstOrDefault();

      DateTime currentDate = DateTime.Today;
      int dayCount = (currentDate - myPlan.DateFrom).Days;

      if (dayCount >= 0 && dayCount < myPlan.Duration)
      {
        DayCustomPlan dayCustomPlan = myPlan.DaysCustomPlan[dayCount];
        CustomDayDTO customDayDTO = _mapper.Map<CustomDayDTO>(dayCustomPlan);

        return customDayDTO;
       }
            else
            {
                CustomDayDTO dayCustomPlan = new CustomDayDTO()
                {
                    Id = 0,
                    CustomMeals = null,
                };
                return dayCustomPlan;
            }
    }
  }
}
