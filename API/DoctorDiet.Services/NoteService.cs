using AutoMapper;
using DoctorDiet.Dto;
using DoctorDiet.DTO;
using DoctorDiet.Models;
using DoctorDiet.Repository.Interfaces;
using DoctorDiet.Repository.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorDiet.Services
{
  public class NoteService
  {
    IGenericRepository<DoctorNotes, int> _doctorNoteRepository;
    IGenericRepository<PatientNotes, int> _patientNoteRepository;
    IUnitOfWork _unitOfWork;
    IMapper _mapper;
    public NoteService(IGenericRepository<DoctorNotes, int> doctorNoteRepository
      , IUnitOfWork unitOfWork, IMapper mapper, IGenericRepository<PatientNotes, int> patientNoteRepository)
    {
      _unitOfWork = unitOfWork;
      _mapper = mapper;
      _doctorNoteRepository = doctorNoteRepository;
      _patientNoteRepository = patientNoteRepository;
    }

    public string AddPatientNote(PatientNotesDTO patientNotesDto)
    {
      PatientNotes patientNotes = _mapper.Map<PatientNotes>(patientNotesDto);
       _patientNoteRepository.Add(patientNotes);
      _unitOfWork.SaveChanges();

      return "Success";
    }
    public List<GetPatientNoteData> GetPateintNotes(string doctorId)
    {
      IQueryable<PatientNotes> patientNotes = _patientNoteRepository
        .Get(x => x.DoctorId == doctorId)
        .Include(x => x.Patient);
      List<GetPatientNoteData> patientNotesDTO = _mapper.ProjectTo<GetPatientNoteData>(patientNotes).ToList();

      return patientNotesDTO;
    }

    public string AddDoctorNote(DoctorNotesDTO doctorNotesDto)
    {
      DoctorNotes Notes = _mapper.Map<DoctorNotes>(doctorNotesDto);
       _doctorNoteRepository.Add(Notes);
      _unitOfWork.SaveChanges();

      return "Success";
    }

    public List<GetDoctorNoteData> GetDoctorNotes(GetDoctorNotesDTO getDoctorNotesDTO)
    {
      IQueryable<DoctorNotes> doctorNotes = _doctorNoteRepository
        .Get(x => x.DoctorId == getDoctorNotesDTO.DoctorId && x.PatientId == getDoctorNotesDTO.PatientId);
      List<GetDoctorNoteData> doctorNoteDatas = _mapper.ProjectTo<GetDoctorNoteData>(doctorNotes).ToList();

      return doctorNoteDatas;
    }

    public PatientNotes UpdateNoteStatus(int Noteid)
    {
      PatientNotes patientNotes = _patientNoteRepository.Get(x => x.Id == Noteid).FirstOrDefault();

      patientNotes.Seen = true;

      _patientNoteRepository.Update(patientNotes);

      _unitOfWork.SaveChanges();

      return patientNotes;
    }
  }
}
