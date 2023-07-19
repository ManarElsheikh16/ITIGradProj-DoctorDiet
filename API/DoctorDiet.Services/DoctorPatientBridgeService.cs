using DoctorDiet.Dto;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using DoctorDiet.Repository.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Services
{
  public class DoctorPatientBridgeService
  {
    IGenericRepository<DoctorPatientBridge, int> _doctorPatirentRepository;
    IUnitOfWork _unitOfWork;

    public DoctorPatientBridgeService(IGenericRepository<DoctorPatientBridge, int> doctorPatientBridge,
      IUnitOfWork unitOfWork) {
      _doctorPatirentRepository = doctorPatientBridge;
      _unitOfWork = unitOfWork;
    }

    public string MakeDoctorPatientBridgeDone(SubscribeDto subscribeDto)
    {
      DoctorPatientBridge doctorPatientBridges = _doctorPatirentRepository
          .Get(p => p.PatientID == subscribeDto.PatientId &&
           p.DoctorID == subscribeDto.DoctorID && p.Status == Status.Confirmed)
          .OrderBy(b => b.Id)
          .LastOrDefault();

       doctorPatientBridges.Status = Status.Done;

      _doctorPatirentRepository.Update(doctorPatientBridges, nameof(DoctorPatientBridge.Status));
      _unitOfWork.SaveChanges();

      return doctorPatientBridges.Status.ToString();
    }

    public string MakeDoctorPatientBridgeCanceled(SubscribeDto subscribeDto)
    {
      DoctorPatientBridge doctorPatientBridges = _doctorPatirentRepository
          .Get(p => p.PatientID == subscribeDto.PatientId &&
           p.DoctorID == subscribeDto.DoctorID && p.Status == Status.Confirmed)
          .OrderBy(b => b.Id)
          .LastOrDefault();

      doctorPatientBridges.Status = Status.Cancled;

      _doctorPatirentRepository.Update(doctorPatientBridges, nameof(DoctorPatientBridge.Status));
      _unitOfWork.SaveChanges();

      return (doctorPatientBridges.Status).ToString();
    }
  }
}
