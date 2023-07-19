using DoctorDiet.Data;
using DoctorDiet.Models;
using DoctorDiet.Repositories.Interfaces;
using DoctorDiet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Repositories.Repositories
{
    public class AdminRepository: IAdminRepository
    {
        Context Context { get; set; }
       
     
        public AdminRepository(Context context)
        {
            this.Context = context; 
        }

        public Admin EditAdminData(string id, AdminDataDTO _adminDataDTO)
        {
           Admin OldAdmin= Context.Admin.Where(e => e.Id == id).FirstOrDefault();
            OldAdmin.FullName = _adminDataDTO.FullName;
            Context.SaveChanges();


            return new Admin();
        } 
    }
}
