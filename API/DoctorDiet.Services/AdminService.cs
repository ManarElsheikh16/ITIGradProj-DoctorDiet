using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using DoctorDiet.Dto;
using DoctorDiet.Repositories.Interfaces;
using AutoMapper;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.DTO;

namespace DoctorDiet.Services
{
    public class AdminService
    {

       IGenericRepository<Admin,string> _repositry;
        IMapper mapper;
        IUnitOfWork unitOfWork;
      


      public AdminService(IGenericRepository<Admin,string> Repositry,IMapper mapper, IUnitOfWork unitOfWork)
        {

            _repositry = Repositry;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
           

        }

        public void AddAdmin(string userid, RegisterAdminDto AdminDto)
        {
          Admin admin = mapper.Map<Admin>(AdminDto);
          admin.Id = userid;

          _repositry.Add(admin);
          unitOfWork.SaveChanges();
        }

    public Admin GetAdminData(string id)
        {
            Admin Admin = _repositry.Get(o => o.Id == id).Include(x=> x.ApplicationUser).FirstOrDefault();
            return Admin;

        }

        public void EditAdminData( AdminDataDTO _adminDataDTO ,params string[] properties)
        {
            using var dataStream = new MemoryStream();
            _adminDataDTO.ProfileImage.CopyTo(dataStream);
            Admin adminMapper=  mapper.Map<Admin>(_adminDataDTO);
            adminMapper.ApplicationUser.ProfileImage = dataStream.ToArray();

             _repositry.Update(adminMapper, properties);
            unitOfWork.SaveChanges();   

        }


    }
}
