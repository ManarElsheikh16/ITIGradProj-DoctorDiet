using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DoctorDiet.Dto;
using DoctorDiet.DTO;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using DoctorDiet.Repository.UnitOfWork;

namespace DoctorDiet.Services
{
    public class AccountService
    {
        IUnitOfWork _UnitOfWork;
        IAccountRepository _accountRepository;
        IMapper _mapper;
        ContactInfoService _contactInfoService;
        public AccountService(IUnitOfWork unitOfWork, IAccountRepository accountRepository,IMapper mapper
          ,ContactInfoService contactInfoService)
        {
            _UnitOfWork = unitOfWork;
            _accountRepository = accountRepository;
      _mapper = mapper;
      _contactInfoService = contactInfoService;
            
        }

        public void AddPatient(Patient Patient)
        {

            _accountRepository.AddPatient(Patient);
            _UnitOfWork.SaveChanges();
        }
        
        public void AddAdmin(string userid, RegisterAdminDto AdminDto)
        {
            Admin admin = _mapper.Map<Admin>(AdminDto);
              admin.Id = userid;

            _accountRepository.AddAdmin(admin);
            _UnitOfWork.SaveChanges();
        }

        public async Task<string> ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {

           string Message = await _accountRepository.ForgetPassAsync(forgetPasswordDTO);

            _UnitOfWork.SaveChanges();

            return Message;
        } 
        
        public string GetQuestionByUserName(string UserName)
        {

           string Question = _accountRepository.GetQuestionByUserName(UserName);

            _UnitOfWork.SaveChanges();

            return Question;
        }

        public void AddDoctor(string userid,RegisterDoctorDto DoctorDto)
        {
            Doctor doctor = _mapper.Map<Doctor>(DoctorDto);
            doctor.Id=userid;

            _accountRepository.AddDoctor(doctor);
            _UnitOfWork.SaveChanges();

            foreach (var item in DoctorDto.ContactInfo)
            {

                _contactInfoService.Add(item,userid);
            }
        }
        
    }
}
