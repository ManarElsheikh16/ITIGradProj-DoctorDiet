using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using AutoMapper;
using DoctorDiet.Dto;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.DTO;
using DoctorDiet.Repositories.Interfaces;
using DoctorDiet.Repositories.Repositories;

namespace DoctorDiet.Services
{
    public class DoctorService
    {
        IGenericRepository<CustomPlan, int> _customPlanRepository;
        IGenericRepository<DayCustomPlan, int> _DayCustomPlanRepository;
        IGenericRepository<MealCustomPlan, int> _MealCustomPlanRepository;
        IGenericRepository<DayMealCustomPlanBridge, int> _DayMealCustomPlanBridgeRepository;
        IGenericRepository<Plan, int> _planRepository;
        IGenericRepository<Doctor, string> _docRepositry;
        IGenericRepository<DoctorPatientBridge, int> _docPatientRepositry;
        IMapper _mapper;
        IUnitOfWork _unitOfWork;
    ContactInfoService _contactInfoService;

        public DoctorService(IGenericRepository<Doctor, string> Repositry,
          IMapper mapper, IUnitOfWork unitOfWork
          ,ContactInfoService contactInfoService,
            IGenericRepository<Plan, int> planRepository,
            IGenericRepository<DoctorPatientBridge, int> docPatientRepositry, IGenericRepository<CustomPlan, int> customPlanRepository,
            IGenericRepository<DayCustomPlan, int> DayCustomPlanRepository,
            IGenericRepository<MealCustomPlan, int> MealCustomPlanRepository,
            IGenericRepository<DayMealCustomPlanBridge, int> DayMealCustomPlanBridgeRepository)
        {

      _contactInfoService = contactInfoService;
            _docRepositry = Repositry;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _docPatientRepositry = docPatientRepositry;
            _customPlanRepository = customPlanRepository;
            _DayCustomPlanRepository = DayCustomPlanRepository;
            _MealCustomPlanRepository = MealCustomPlanRepository;
            _DayMealCustomPlanBridgeRepository = DayMealCustomPlanBridgeRepository;
            _planRepository = planRepository;

        }

        public DoctorGetDataDto GetDoctorData(string id)
        {
            Doctor doctor = _docRepositry.Get(d => d.Id == id)
                .Include(d=>d.ApplicationUser)
                .Include(d=>d.ContactInfo)
                .FirstOrDefault();

            DoctorGetDataDto doctorDataDTO = _mapper.Map<DoctorGetDataDto>(doctor);
            return doctorDataDTO;

        }
        public Doctor GetById(string Id)
        {
            Doctor doctor = _docRepositry.Get(d=>d.Id==Id)
            .Include(d=>d.ApplicationUser)
            .FirstOrDefault();
            return doctor;
        }

   

    public void EditDoctorData(DoctorDataDTO doctorData, params string[] properties)
    {

      if (doctorData.contactInfo != null)
      {
        foreach (ContactInfoDTO phone in doctorData.contactInfo)
        {
          _contactInfoService.EditContactInfo(phone, nameof(phone.contactInfo));
        }
      }
      Doctor doctorMapper = _mapper.Map<Doctor>(doctorData);

      _docRepositry.Update(doctorMapper, properties);
      _unitOfWork.SaveChanges();
    }


    public List<ShowDoctorDTO> GetListOfDoctors()
        {
            IQueryable<Doctor> doctors = _docRepositry.GetAll().Include(d=>d.ApplicationUser);
            List<ShowDoctorDTO> DoctorsDto = _mapper.ProjectTo<ShowDoctorDTO>(doctors).ToList();

            return DoctorsDto;
        }

    

        public string GetDocIdWithStatusConfirmedByPatientId(string patientId)
        {
            DoctorPatientBridge doctorPatient=_docPatientRepositry
            .Get(pat=>pat.PatientID==patientId && pat.Status==Status.Confirmed)
            .OrderBy(d=>d.Id)
            .LastOrDefault();
          if(doctorPatient != null) { 
            string DocId = doctorPatient.DoctorID;

            return DocId;
          }
           return "Not Found";
        }

    public byte[] GetDoctorImg(string id)
    {
      byte[] img = _docRepositry.Get(d => d.Id == id)
          .Include(d => d.ApplicationUser)
          .Select(i => i.ApplicationUser.ProfileImage).FirstOrDefault();

      return img;

    }

    public void AddDoctor(string userid, RegisterDoctorDto DoctorDto)
    {
      Doctor doctor = _mapper.Map<Doctor>(DoctorDto);
      doctor.Id = userid;

      _docRepositry.Add(doctor);
      _unitOfWork.SaveChanges();

      foreach (var item in DoctorDto.ContactInfo)
      {

        _contactInfoService.Add(item, userid);
      }
    }
        public string AddCustomPlanToSpecificPatient(ChoosePlanDTO choosePlanDTO)
        {


            CustomPlan OldCustomPlan = _customPlanRepository.Get(c => c.PatientId == choosePlanDTO.PatientId)
                .OrderBy(c => c.Id)
                .LastOrDefault();

            OldCustomPlan.IsAvaliable = false;
            _customPlanRepository.Update(OldCustomPlan, nameof(CustomPlan.IsAvaliable));
            _unitOfWork.SaveChanges();


            Plan plan = _planRepository.Get(plan => plan.Id == choosePlanDTO.planId).Include(doc => doc.Doctor).Include(day => day.Days).ThenInclude(dm => dm.DayMeal).ThenInclude(meal => meal.Meal).FirstOrDefault();

            CustomPlan customPlan = new CustomPlan();
            if (plan != null)
            {
                customPlan = _mapper.Map<CustomPlan>(plan);
                customPlan.DoctorName = plan.Doctor.FullName;
                customPlan.DateFrom = DateTime.Now;
                customPlan.DateTo = DateTime.Now.AddDays(plan.Duration);
                customPlan.PatientId = choosePlanDTO.PatientId;
                customPlan.IsAvaliable = true;
                customPlan.goal = plan.goal;

        _customPlanRepository.Add(customPlan);
            }
            else
            {
                customPlan = null;
                return "invalidPlan";
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


      return "Success";

        }

    }
}
