using DoctorDiet.Dto;
using DoctorDiet.Models;
using DoctorDiet.Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Repositories.Interfaces
{
    public interface IAdminRepository
    {
        Admin EditAdminData(string id, AdminDataDTO _adminDataDTO);
    }
}
