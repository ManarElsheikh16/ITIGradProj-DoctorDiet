using AutoMapper;
using AutoMapper.QueryableExtensions;
using DoctorDiet.Dto;
using DoctorDiet.DTO;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using DoctorDiet.Repository.Repositories;
using DoctorDiet.Repository.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Services
{
    public class PlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Day, int> _dayRepository;
        private readonly IGenericRepository<Plan, int> _planRepository;
        private readonly IGenericRepository<Meal, int> _mealRepository;
        private readonly IGenericRepository<DayMealBridge, int> _DayMealBridgeRepository;
        IGenericRepository<AllergicsPlan, int> _AllergicsRepository;
        public PlanService( IUnitOfWork unitOfWork, IMapper mapper
              ,
          IGenericRepository<Meal, int> mealRepository,
          IGenericRepository<DayMealBridge, int> DayMealBridgeRepository,
          IGenericRepository<AllergicsPlan, int> AllergicsRepository,
          IGenericRepository<Day, int> dayrepository,
            IGenericRepository<Plan, int> planRepository)
        {
            _unitOfWork = unitOfWork;
            _dayRepository = dayrepository;
            this._mealRepository = mealRepository;

            this._mapper = mapper;
            this._DayMealBridgeRepository = DayMealBridgeRepository;
            _AllergicsRepository = AllergicsRepository;
            _planRepository = planRepository;
        }

        
        public PlanDataDTO GetPlanById(int id)
        {
            Plan Plan = GetPlans(p => p.Id == id).FirstOrDefault();
            PlanDataDTO planDataDTO = _mapper.Map<PlanDataDTO>(Plan);
            return planDataDTO;
        }
        public IQueryable<Plan> GetPlans(Expression<Func<Plan, bool>> expression)
        {
            return _planRepository.Get(expression);
        }

       

        public void AddPlan(AddPlanDTO planDto)
        {
            Plan plan = _mapper.Map<Plan>(planDto);
            _planRepository.Add(plan);
            _unitOfWork.SaveChanges();
            if (planDto.Days != null)
            {

                foreach (DayDTO dayDTO in planDto.Days)
                {
                    Day day = new Day()
                    {
                        PlanId = plan.Id
                    };
                    _dayRepository.Add(day);
                    _unitOfWork.SaveChanges();
                    foreach (MealDTO mealDTO in dayDTO.Meals)
                    {

                        Meal meal = _mapper.Map<Meal>(mealDTO);
                        meal.Image = mealDTO.Image;
                        _mealRepository.Add(meal);
                        _unitOfWork.SaveChanges();


                        DayMealBridge dayMealBridge = new DayMealBridge()
                        {
                            DayId = day.Id,
                            MealId = meal.Id,
                        };
                        _DayMealBridgeRepository.Add(dayMealBridge);
                        _unitOfWork.SaveChanges();

                    }

                }
                if (planDto.Allergics != null)
                {
                    foreach (AllergicsPlanDto allergics in planDto.Allergics)
                    {
                        AllergicsPlan allergicsPlan = new AllergicsPlan()
                        {
                            Name = allergics.Name,
                            PlanId = plan.Id
                        };
                        _AllergicsRepository.Add(allergicsPlan);
                        _unitOfWork.SaveChanges();
                    }
                }
            }

        }

       

        public void DeletePlan(int id)
        {
            _planRepository.Delete(id);
            _unitOfWork.SaveChanges();
        }

        public List<PlanDataDTO> GetPlanByDoctorId(string doctorId)
        {

            IQueryable<Plan> plans = _planRepository.Get(doc => doc.DoctorID == doctorId);
            IQueryable<PlanDataDTO> patientDTOs = plans.ProjectTo<PlanDataDTO>(_mapper.ConfigurationProvider);


            return patientDTOs.ToList();

        }

        public List<Day> GetDaysDTOByPlanId(int planId)
        {

            List<Day> days = _dayRepository.Get(d => d.PlanId == planId)
           .ToList();
            return days;

        }



        public List<MealDTO> GetMealsByDayId(int dayId)
        {
            List<MealDTO> mealsDto = new List<MealDTO>();
            Day days = _dayRepository.Get(d => d.Id == dayId).Include(dm => dm.DayMeal).ThenInclude(m => m.Meal).FirstOrDefault();
            DayDTO DaysDTO = _mapper.Map<DayDTO>(days);

            mealsDto = DaysDTO.Meals;

            return mealsDto;

        }

        public MealDTO GetMealById(int mealId)
        {
            Meal meal = _mealRepository.Get(m => m.Id == mealId).FirstOrDefault();
            MealDTO mealDto = _mapper.Map<MealDTO>(meal);

            return mealDto;

        }

        public Meal UpdateMealInPlan(UpdateMealDTO UodateMealDTO, params string[] properties)
        {
            Meal meal = _mapper.Map<Meal>(UodateMealDTO);
            using var dataStream = new MemoryStream();
            if (UodateMealDTO.Image != null)
            {
                UodateMealDTO.Image.CopyTo(dataStream);

                meal.Image = dataStream.ToArray();
            }

              _mealRepository.Update(meal, properties);
           

            _unitOfWork.SaveChanges();

            return meal;
        }

        public Plan UpdatePlan(UpdatePlanDTO updatePlanDTO,params string[] properties)
        {
            Plan plan=_mapper.Map<Plan>(updatePlanDTO);
            _planRepository.Update(plan, properties);
            _unitOfWork.SaveChanges();

            return plan;
        }



    }
}
