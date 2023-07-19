using DoctorDiet.Dto;
using DoctorDiet.DTO;
using DoctorDiet.Models;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DoctorDiet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : Controller
    {

        NoteService _noteService;

        DoctorService _doctorService;
        IUnitOfWork unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        public DoctorController(DoctorService doctorService, IUnitOfWork unitOfWork,
          UserManager<ApplicationUser> userManager, NoteService noteService)
        {
            _doctorService = doctorService;
            this.unitOfWork = unitOfWork;
            _userManager = userManager;
            _noteService = noteService;
        }


        [HttpGet("doctorid")]
        public IActionResult GetDoctorById(string doctorid)
        {
            DoctorGetDataDto doctor = _doctorService.GetDoctorData(doctorid);

            return Ok(doctor);

        }


        [HttpPost("GetDoctorNotes")]
        public IActionResult GetDoctorNotes(GetDoctorNotesDTO getDoctorNotesDTO)
        {
            List<GetDoctorNoteData> DoctorNotes = _noteService.GetDoctorNotes(getDoctorNotesDTO);


            return Ok(DoctorNotes);
        }

        [HttpPost("AddDoctorNote")]
        public IActionResult AddNote(DoctorNotesDTO doctorNotesDto)
        {
            string status = _noteService.AddDoctorNote(doctorNotesDto);
            unitOfWork.CommitChanges();
            return Ok(new { Status = status });
        }


        [HttpPut("EditDoctorData")]
        public IActionResult EditDoctorData(DoctorDataDTO doctorData)
        {

            string[] propertiesArray = doctorData.properties.ToArray();
            if (ModelState.IsValid)
            {
                _doctorService.EditDoctorData(doctorData, propertiesArray);
                unitOfWork.CommitChanges();
                return Ok(new
                {
                    msg = "done"
                });
            }


            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet("GetAllDoctors")]
        public IActionResult GetAllDoctor()
        {
            if (ModelState.IsValid)
            {
                List<ShowDoctorDTO> doctors = _doctorService.GetListOfDoctors();
                return Ok(doctors);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet("GetDocIdWithStatusConfirmedByPatientId/{PatientId}")]
        public IActionResult GetDocIdWithStatusConfirmedByPatientId(string PatientId)
        {
            if (ModelState.IsValid)
            {
                string doctorId = _doctorService.GetDocIdWithStatusConfirmedByPatientId(PatientId);
                return Ok(new { Docid = doctorId });
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpGet("Done")]

        public IActionResult Done()
        {
            return Ok("Done");
        }


        [HttpPost("ChangePassowrd")]
        public async Task<IActionResult> ChangePassowrd(DoctorNewPasswordDTO newPassowrd)
        {

            if (ModelState.IsValid)
            {
                Doctor doctor = _doctorService.GetById(newPassowrd.DoctorId);

                if (doctor != null)
                {
                    PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
                    var newPasswordHash = passwordHasher.HashPassword(doctor.ApplicationUser, newPassowrd.Password);

                    doctor.ApplicationUser.PasswordHash = newPasswordHash;
                    var result = await _userManager.UpdateAsync(doctor.ApplicationUser);

                    RegisterDto registerDto = new RegisterDto();
                    unitOfWork.CommitChanges();

                    if (result.Succeeded)
                    {
                        registerDto.Message = "Success";
                        return Ok(registerDto);
                    }
                    else
                        registerDto.Message = "Failed";
                    return BadRequest(registerDto);
                }

            }
            return BadRequest(ModelState);

        }


        [HttpGet("GetDoctorImg")]
        public IActionResult GetDoctorImg(string doctorid)
        {
            byte[] img = _doctorService.GetDoctorImg(doctorid);

            return Ok(new
            {
                img = img
            });

        }

        [HttpPost("AddCustomPlanToSpecificPatient")]
        public IActionResult AddCustomPlanToSpecificPatient(ChoosePlanDTO choosePlanDto)
        {
            if (ModelState.IsValid)
            {
                string status = _doctorService.AddCustomPlanToSpecificPatient(choosePlanDto);
                unitOfWork.CommitChanges();

                return Ok(new { Status = status });
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

    }


}
