using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using DoctorDiet.DTO;
using AutoMapper;
using DoctorDiet.Dto;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.Repositories.Interfaces;
using AutoMapper.QueryableExtensions;
using DoctorDiet.Repository.Repositories;

namespace DoctorDiet.Services
{
    public class PatientService
    { 
    
        IGenericRepository<Patient, string> _Patientrepositry;
        IGenericRepository<DoctorPatientBridge, int> _doctorPatirentRepository;
        IMapper _mapper;
        NoEatService _NoEatService;
        IUnitOfWork _unitOfWork;
        CustomPlanService _customPlanService;
        DoctorPatientBridgeService _doctorPatientBridgeService;

    public PatientService(IGenericRepository<Patient, string> Repositry,
            IMapper mapper, NoEatService NoEatService, 
             IUnitOfWork unitOfWork
            , CustomPlanService customPlanService,
             DoctorPatientBridgeService doctorPatientBridgeService,
            IGenericRepository<DoctorPatientBridge, int> doctorPatirentRepository)
        {
            _Patientrepositry = Repositry;
            _mapper = mapper;
            _NoEatService = NoEatService;
            _unitOfWork = unitOfWork;
            _customPlanService = customPlanService;
            _doctorPatirentRepository = doctorPatirentRepository;
      _doctorPatientBridgeService = doctorPatientBridgeService;
        }


        public Patient GetPatientData(string id)
        {
            Patient patient = _Patientrepositry.Get(o => o.Id == id)
                 .Include(x => x.ApplicationUser)
                 .Include(c => c.CustomPlans.OrderByDescending(cp => cp.Id).Take(1))
                 .FirstOrDefault();

            return patient;

        }
        public Patient AddPatient(RegisterPatientDto registerPatientDto)
        {
            Patient patient = _mapper.Map<Patient>(registerPatientDto);
            patient.Id = registerPatientDto.PatientId;
            _Patientrepositry.Add(patient);

            foreach (string noEat in registerPatientDto.noEats)
            {
                NoEat noeat = new NoEat
                {

                    PatientId = patient.Id,
                    Name = noEat,

                };
                _NoEatService.AddNoEat(noeat);
            }
            return patient;

        }
       

        

        public Patient GetById(string Id)
    {
      Patient patient = _Patientrepositry.Get(d => d.Id == Id)
      .Include(d => d.ApplicationUser)
      .FirstOrDefault();
      return patient;
    }

        public string Confirm(SubscribeDto subscribeDto)
        {
            DoctorPatientBridge isConfirmed = GetStatusIfComfirmed(subscribeDto.PatientId);
            if (isConfirmed.Status.ToString() != "Confirmed")
            {
                Patient patient = _Patientrepositry.GetAll()
                         .FirstOrDefault(pat => pat.Id == subscribeDto.PatientId);
                CustomPlan customPlan =  _customPlanService.AddCustomPlan(patient) ;
                if (customPlan != null)
                {
                    DoctorPatientBridge doctorPatientBridge = _doctorPatirentRepository.
                    Get(d => d.DoctorID == subscribeDto.DoctorID && d.PatientID == subscribeDto.PatientId).
                    OrderBy(d=>d.Id).LastOrDefault();

                    doctorPatientBridge.Status = Status.Confirmed;
                    _doctorPatirentRepository.Update(doctorPatientBridge, nameof(DoctorPatientBridge.Status));
                    _unitOfWork.SaveChanges();

                    return (doctorPatientBridge.Status).ToString();
                }
                else
                {
                    return string.Empty;
                }

            }
            else
            {
                return "isconfirmed";
            }
        }
        public string GetStatus(string patientid)
        {
            DoctorPatientBridge doctorPatientBridges = _doctorPatirentRepository
                .Get(p => p.PatientID == patientid
                 && (p.Status == Status.Waiting||p.Status==Status.Confirmed))
                .FirstOrDefault();

            if (doctorPatientBridges != null)
            {
                return doctorPatientBridges.Status.ToString();
            }
            else
            {
                return "Not Confirmed in plan";
            }


        }
        public DoctorPatientBridge GetStatusIfComfirmed(string patientid)
        {
            DoctorPatientBridge doctorPatientBridges = _doctorPatirentRepository
                .Get(p => p.PatientID == patientid)
                .OrderBy(b => b.Id)
                .LastOrDefault();


            return doctorPatientBridges;


        }

            public string Reject(SubscribeDto subscribeDto) 
        {
            DoctorPatientBridge doctorPatientBridge = _doctorPatirentRepository.
               Get(d => d.DoctorID == subscribeDto.DoctorID && d.PatientID == subscribeDto.PatientId
               && d.Status==Status.Waiting)
               .FirstOrDefault();

            doctorPatientBridge.Status = Status.Rejected;


            _doctorPatirentRepository.Update(doctorPatientBridge, nameof(DoctorPatientBridge.Status), nameof(DoctorPatientBridge.DoctorID));
            string status= (doctorPatientBridge.Status).ToString();
            _unitOfWork.SaveChanges();

            return status;
        }
        public string CancelStatus(SubscribeDto subscribeDto)
        {
            string status = _doctorPatientBridgeService.MakeDoctorPatientBridgeCanceled(subscribeDto);

            string avaliability = _customPlanService.MakeCustomPlanExpired(subscribeDto.PatientId);
            if (avaliability != "")
            {
                return status;
            }
            else
            {
                return "Empty";
            }

        }
        public string Subscription(SubscribeDto subscribeDto)
        {
          Patient patient = _Patientrepositry.Get(patient => patient.Id == subscribeDto.PatientId)
        .Include(p=>p.ApplicationUser)
        .FirstOrDefault();

          DoctorPatientBridge doctorPatientBridge = new DoctorPatientBridge
          {
            DoctorID = subscribeDto.DoctorID,
            PatientID = subscribeDto.PatientId,
            Status = Status.Waiting,
          };
          _doctorPatirentRepository.Add(doctorPatientBridge);


           _unitOfWork.SaveChanges();

            return "Subscription";

        }

        public UserDataDTO GetPatientDataDTO(string id)
        {
            Patient patient = GetPatientData(id);
            var patientdto = _mapper.Map<UserDataDTO>(patient);
            if (patient.CustomPlans.Count() != 0)
            {
                patientdto.DateFrom = patient.CustomPlans[0].DateFrom;
            }
            return patientdto;

        }

        public IEnumerable<PatientDTO> GetPatientsByDoctorIdWithStatusConfirmed(string doctorID)
        {
            var DoctorPatientsBridge = _doctorPatirentRepository
            .Get(p => p.DoctorID == doctorID && p.Status.Equals(Status.Confirmed))
            .Include(b=>b.Patient)
            .ThenInclude(p=>p.ApplicationUser);

            IQueryable<PatientDTO> patientDTOs = DoctorPatientsBridge.ProjectTo<PatientDTO>(_mapper.ConfigurationProvider);

            return patientDTOs.ToList();
        }

        public IEnumerable<PatientDTO> GetPatientsByDoctorIdWithStatusWaiting(string doctorID)
        {

            IQueryable<DoctorPatientBridge> DoctorPatientsBridge = _doctorPatirentRepository
            .Get(p => p.DoctorID == doctorID && p.Status.Equals(Status.Waiting));

            IQueryable<PatientDTO> patientDTOs = _mapper.ProjectTo<PatientDTO>(DoctorPatientsBridge);

            return patientDTOs.ToList();
        }
        

        public void EditPatientData(EditPatientDto patientData, params string[] properties)
        {


            Patient patientMapper = _mapper.Map<Patient>(patientData);
            if (patientData.ProfileImage != null)
            {
                using var dataStream = new MemoryStream();
                patientData.ProfileImage.CopyTo(dataStream);
                         
                patientMapper.ApplicationUser.ProfileImage = dataStream.ToArray();
            }
            if (properties.Contains("Weight") || properties.Contains("ActivityRates"))
            {
                double BMR = 0.0;

                int MaxHisActivityRate = 0;
                int MinHisActivityRate = 0;

                double MinCals = 0.0;
                double MaxCals = 0.0;

                if (patientMapper.Gender.ToString() == "Male")
                {
                    BMR = 24 * 1 * patientMapper.Weight;

                    if (patientMapper.ActivityRates.Contains("veryHigh"))
                    {
                        MaxHisActivityRate = 120;
                        MinHisActivityRate = 90;
                    }

                    else if (patientMapper.ActivityRates.Contains("high"))
                    {
                        MaxHisActivityRate = 80;
                        MinHisActivityRate = 65;
                    }

                    else if (patientMapper.ActivityRates.Contains("regular"))
                    {
                        MaxHisActivityRate = 70;
                        MinHisActivityRate = 50;
                    }

                    else if (patientMapper.ActivityRates.Contains("low"))
                    {
                        MaxHisActivityRate = 40;
                        MinHisActivityRate = 25;
                    }


                }

                else if (patientMapper.Gender.ToString() == "Female")
                {
                    BMR = 24 * 0.9 * patientMapper.Weight;

                    if (patientMapper.ActivityRates.Contains("veryHigh"))
                    {
                        MaxHisActivityRate = 100;
                        MinHisActivityRate = 80;
                    }

                    else if (patientMapper.ActivityRates.Contains("high"))
                    {
                        MaxHisActivityRate = 70;
                        MinHisActivityRate = 50;
                    }

                    else if (patientMapper.ActivityRates.Contains("regular"))
                    {
                        MaxHisActivityRate = 60;
                        MinHisActivityRate = 40;
                    }

                    else if (patientMapper.ActivityRates.Contains("low"))
                    {
                        MaxHisActivityRate = 35;
                        MinHisActivityRate = 25;
                    }
                }

                MinCals = BMR * MinHisActivityRate / 100;
                MaxCals = BMR * MaxHisActivityRate / 100;

                patientMapper.MinCalories = (int)(MinCals + MaxCals);

                patientMapper.MaxCalories = (int)(MaxCals + MaxCals);


                Array.Resize(ref properties, properties.Length + 2);
                properties[properties.Length - 1] = "MinCalories";
                properties[properties.Length - 2] = "MaxCalories";


            }


            _Patientrepositry.Update(patientMapper, properties);
            _unitOfWork.SaveChanges();
        }


        

    }
}
