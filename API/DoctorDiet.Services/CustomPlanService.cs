using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using System.Linq.Expressions;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.Data;
using AutoMapper;
using DoctorDiet.Dto;
using AutoMapper.QueryableExtensions;

namespace DoctorDiet.Services
{
    public class CustomPlanService
    {
    IGenericRepository<CustomPlan, int> _CustomPlanRepository;
        IUnitOfWork _unitOfWork;
        IGenericRepository<Plan, int> _PlanRepository;
        IGenericRepository<DayCustomPlan, int> _DayCustomPlanRepository;
        IGenericRepository<MealCustomPlan, int> _MealCustomPlanRepository;
        IGenericRepository<DayMealCustomPlanBridge, int> _DayMealCustomPlanBridgeRepository;
        IMapper _mapper;
    DoctorPatientBridgeService _doctorPatientBridgeService;

    public CustomPlanService(IMapper mapper, IGenericRepository<CustomPlan, int> repository,
            IUnitOfWork unitOfWork, IGenericRepository<Plan, int> PlanRepository,
            IGenericRepository<DayCustomPlan, int> DayCustomPlanRepository,
            IGenericRepository<MealCustomPlan, int> _MealCustomPlanRepository,
           IGenericRepository<DayMealCustomPlanBridge, int> DayMealCustomPlanBridgeRepository,
           DoctorPatientBridgeService doctorPatientBridgeService)
        {
      _DayMealCustomPlanBridgeRepository = DayMealCustomPlanBridgeRepository;
      _CustomPlanRepository = repository;
            _unitOfWork = unitOfWork;
            _PlanRepository = PlanRepository;
            _DayCustomPlanRepository = DayCustomPlanRepository;
            this._MealCustomPlanRepository = _MealCustomPlanRepository;
             _mapper = mapper;
      _doctorPatientBridgeService = doctorPatientBridgeService;
        }



 

    public List<ShowCustomPlanDto> GetPatientHistory(string patientID)
    {
      IQueryable<CustomPlan> customPlans = _CustomPlanRepository.Get(c => c.PatientId == patientID);

      List<ShowCustomPlanDto> customPlanDtos = customPlans.ProjectTo<ShowCustomPlanDto>(_mapper.ConfigurationProvider).ToList();


      return customPlanDtos;
    }
    public CustomPlan AddCustomPlan(Patient CurrentPatient)
        {
            CustomPlan customPlan = new CustomPlan();
            Plan plan = new Plan();
            if (CurrentPatient.Goal.ToString() == "weightLoss")
            {
                plan = _PlanRepository.GetAll()
                    .Include(d => d.Doctor)
                    .Include(d => d.Days)
                    .ThenInclude(m => m.DayMeal)
                    .ThenInclude(M => M.Meal)
                    .FirstOrDefault(p => p.CaloriesTo <= CurrentPatient.MinCalories
                    && p.goal == CurrentPatient.Goal && p.gender == CurrentPatient.Gender);
            }
            else if (CurrentPatient.Goal.ToString() == "weightGain")
            {
                plan = _PlanRepository.GetAll()
                    .Include(d => d.Doctor)
                    .Include(d => d.Days)
                    .ThenInclude(m => m.DayMeal)
                    .ThenInclude(M => M.Meal)
                    .FirstOrDefault(p => p.CaloriesTo >= CurrentPatient.MaxCalories
                    && p.goal == CurrentPatient.Goal && p.gender == CurrentPatient.Gender);
            }
            else if (CurrentPatient.Goal.ToString() == "muscleBuilding")
            {
                plan = _PlanRepository.GetAll()
                    .Include(d => d.Doctor)
                    .Include(d => d.Days)
                    .ThenInclude(m => m.DayMeal)
                    .ThenInclude(M => M.Meal)
                    .FirstOrDefault(p => p.CaloriesFrom <= CurrentPatient.MinCalories && p.CaloriesTo >= CurrentPatient.MaxCalories
                    && p.goal == CurrentPatient.Goal && p.gender == CurrentPatient.Gender);
            }

            if (plan != null)
            {
            
                customPlan = _mapper.Map<CustomPlan>(plan);

                customPlan.DoctorName = plan.Doctor.FullName;
                customPlan.DateFrom = DateTime.Now;
                customPlan.DateTo = DateTime.Now.AddDays(plan.Duration);
                customPlan.PatientId = CurrentPatient.Id;
                customPlan.IsAvaliable = true;
                customPlan.goal = plan.goal;

                _CustomPlanRepository.Add(customPlan);
            }
            else
            {
                customPlan = null;
                return customPlan;
            }
            _unitOfWork.SaveChanges();

      int duration = plan.Duration;
      int numDays = plan.Days.Count();
      int count = 0;

      while (duration > 0)
      {
        foreach (Day day in plan.Days)
        {
          if (duration <= 0)
            break;

          DayCustomPlan dayCustomPlan = new DayCustomPlan();
          dayCustomPlan.CustomPlanId = customPlan.Id;
          _DayCustomPlanRepository.Add(dayCustomPlan);
          _unitOfWork.SaveChanges();

          foreach (DayMealBridge dayMeal in day.DayMeal)
          {
            MealCustomPlan mealCustomPlan = _mapper.Map<MealCustomPlan>(dayMeal.Meal);
            _MealCustomPlanRepository.Add(mealCustomPlan);
            _unitOfWork.SaveChanges();

            DayMealCustomPlanBridge dayMealBridge = new DayMealCustomPlanBridge()
            {
              DayId = dayCustomPlan.Id,
              MealId = mealCustomPlan.Id,
            };
            _DayMealCustomPlanBridgeRepository.Add(dayMealBridge);
            _unitOfWork.SaveChanges();
          }

          duration--;
          count++;
        }
      }

      if (count < numDays)
      {
        int daysToRemove = numDays - count;
        plan.Days.RemoveRange(count, daysToRemove);
      }

      return customPlan;

        }


    public CurrentCustomPlanDto GetCurrentCustomPlanId(SubscribeDto subscribeDto)
    {
      CustomPlan currentCustomPlan = _CustomPlanRepository.Get(c => c.PatientId == subscribeDto.PatientId)
        .OrderBy(c=>c.Id)
        .LastOrDefault();
      if (DateTime.Now <= currentCustomPlan.DateTo)
      {
        CurrentCustomPlanDto currentCustomPlanDto = new CurrentCustomPlanDto();
        currentCustomPlanDto.id = currentCustomPlan.Id;

        return currentCustomPlanDto;
      }
      else
      {
        _doctorPatientBridgeService.MakeDoctorPatientBridgeDone(subscribeDto);
        currentCustomPlan.IsAvaliable = false;

        _CustomPlanRepository.Update(currentCustomPlan, nameof(CustomPlan.IsAvaliable));
        _unitOfWork.SaveChanges();
        CurrentCustomPlanDto currentCustomPlanDto = new CurrentCustomPlanDto();
        currentCustomPlanDto.id =0;

        return currentCustomPlanDto;
      }

    }
    

        public MealCustomPlan UpdateMealInCustomPlan(UpdateMealDTO UodateMealDTO, params string[] properties)
        {
            MealCustomPlan mealCustomPlan = _mapper.Map<MealCustomPlan>(UodateMealDTO);

            using var dataStream = new MemoryStream();
            
            if (UodateMealDTO.Image != null)
            {
                UodateMealDTO.Image.CopyTo(dataStream);

                mealCustomPlan.Image = dataStream.ToArray();
          
            }

           _MealCustomPlanRepository.Update(mealCustomPlan, properties);
         
            _unitOfWork.SaveChanges();
            return mealCustomPlan;
        }

        public List<DayCustomPlan> GetDayCustomPlanByCusPlanId(int customPlanId)
        {

            List<DayCustomPlan> DaysCustomPlan = _DayCustomPlanRepository.GetAll()
            .Where(cusPlanId => cusPlanId.CustomPlanId == customPlanId)
            .ToList();


            return DaysCustomPlan;
        }

        public List<CustomMealsDTO> GetMealsByCusDayId(int DayCusId)
        {
            DayCustomPlan dayCustomPlan = _DayCustomPlanRepository.Get(X => X.Id == DayCusId)
           .Include(dm => dm.DayMealCustomPlanBridge).ThenInclude(m => m.MealCustomPlan)
           .FirstOrDefault();

            CustomDayDTO CustomDayDTO = _mapper.Map<CustomDayDTO>(dayCustomPlan);

            return CustomDayDTO.CustomMeals;
        }


        public CustomMealsDTO GetCustomMealByID(int id)
        {

            MealCustomPlan meals = _MealCustomPlanRepository.Get(m=>m.Id== id).FirstOrDefault();
            CustomMealsDTO customMealsDTO =_mapper.Map<CustomMealsDTO>(meals);
            return customMealsDTO;
        }



    public string MakeCustomPlanExpired(string patientid)
    {
      CustomPlan currentCustomPlan = _CustomPlanRepository.Get(c => c.PatientId == patientid
      && DateTime.Now <= c.DateTo && c.IsAvaliable == true)
        .OrderBy(c => c.Id)
        .LastOrDefault();
            if (currentCustomPlan != null)
            {
                currentCustomPlan.IsAvaliable = false;


                _CustomPlanRepository.Update(currentCustomPlan, nameof(CustomPlan.IsAvaliable));
                _unitOfWork.SaveChanges();

                return "Expired";
            }
            else
            {
                return "EmptyPlan!";
            }
    }



  }
}
