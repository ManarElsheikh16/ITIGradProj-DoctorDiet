using DoctorDiet.Data;
using DoctorDiet.Dto;
using DoctorDiet.Models;
using DoctorDiet.Repositories.Interfaces;
using DoctorDiet.Repositories.Repositories;
using DoctorDiet.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Repository.Reposetories
{
    public class AccountRepository: IAccountRepository
    {
        Context _context;
        private readonly UserManager<ApplicationUser> userManager;
        IGenericRepository<Patient,string> _patientRepository;
        public AccountRepository(Context context,
            UserManager<ApplicationUser> userManager,
            IGenericRepository<Patient, string> patientRepository)
        {
            _context = context;
            this.userManager = userManager;
            this._patientRepository = patientRepository;
        }




        public void AddAdmin(Admin Admin)
        {
            _context.Admin.Add(Admin);

        }

        public void AddPatient(Patient Patient)
        {
            _context.Patient.Add(Patient);

        }

        public void AddDoctor(Doctor Doctors)
        {
            _context.Doctors.Add(Doctors);

        }

        public async Task<string> ForgetPassAsync(ForgetPasswordDTO forgetPasswordDTO)
        {
           ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == forgetPasswordDTO.UserName);

            if (user != null)
            {
                var userRole = _context.UserRoles.FirstOrDefault(u => u.UserId == user.Id);

                var role = _context.Roles.FirstOrDefault(v => v.Id == userRole.RoleId);

                if (role.Name == "Patient")
                {
                    Patient patient = _context.Patient.FirstOrDefault(x => x.Id == user.Id);

                    if(patient.Answer == forgetPasswordDTO.Answer)
                    {
                        PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
                        var newPasswordHash = passwordHasher.HashPassword(user, forgetPasswordDTO.NewPass);

                         user.PasswordHash = newPasswordHash;
                        var result =  await userManager.UpdateAsync(user);
                        return "Password Changed Successfully";
                    }
                    else
                    {
                        return "Wrong Answer";
                    }
                }

                else if (role.Name == "Doctor")
                {
                    Doctor doctor = _context.Doctors.FirstOrDefault(x => x.Id == user.Id);

                    if (doctor != null)
                    {
                        if (doctor.Answer == forgetPasswordDTO.Answer)
                        {
                            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
                            var newPasswordHash = passwordHasher.HashPassword(user, forgetPasswordDTO.NewPass);

                            user.PasswordHash = newPasswordHash;
                            var result = await userManager.UpdateAsync(user);

                            return "Password Changed Successfully";
                        }
                        else
                        {
                            return "Wrong Answer";
                        }
                    }
                }

            }
                return "There's No User With This UserName";
        }

        public string GetQuestionByUserName(string UserName)
        {
            ApplicationUser user = _context.Users.FirstOrDefault(u => u.UserName == UserName);

            if (user != null)
            {
                

                var userRole = _context.UserRoles.FirstOrDefault(u => u.UserId == user.Id);

                var role = _context.Roles.FirstOrDefault(v => v.Id == userRole.RoleId);

                if (role.Name == "Patient")
                {
                    Patient patient = _context.Patient.FirstOrDefault(x => x.Id == user.Id);

                    if (patient != null)
                    {
                        return patient.Question;
                    }

                }

                else if (role.Name == "Doctor")
                {
                    Doctor doctor = _context.Doctors.FirstOrDefault(x => x.Id == user.Id);

                    if (doctor != null)
                    {
                        return doctor.Question;
                    }
                    
                }

            }
            return "لا يوجد حساب بهذا الاسم";
        }
    }
}
