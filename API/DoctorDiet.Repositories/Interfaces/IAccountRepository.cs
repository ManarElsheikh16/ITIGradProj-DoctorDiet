using DoctorDiet.Dto;
using DoctorDiet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<string> ForgetPassAsync(ForgetPasswordDTO forgetPasswordDTO);
        string GetQuestionByUserName(string UserName);
        void AddPatient(Patient Patient);
        
        void AddAdmin(Admin Admin);

        void AddDoctor(Doctor Doctor);
  
    }
}
