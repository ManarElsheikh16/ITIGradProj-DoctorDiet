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
    public class PatientController : Controller
        {
            NoteService _noteService;
            PatientService _patientService;
            CustomPlanService _customPlanService;
            IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;


    public PatientController(PatientService patientService, IUnitOfWork unitOfWork ,
      UserManager<ApplicationUser> userManager, NoteService noteService,CustomPlanService customPlanService)
            {
               _patientService = patientService;
               _unitOfWork = unitOfWork;
               _userManager = userManager;
               _noteService = noteService;
               _customPlanService = customPlanService;
            }

        


        [HttpGet("GetPatientHistory/{PatientId}")]
        public IActionResult GetPatientCustomPlanHistory(string PatientId)
        {
            if (ModelState.IsValid)
            {
                List<ShowCustomPlanDto> customsPlans = _customPlanService.GetPatientHistory(PatientId);
                return Ok(customsPlans);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPost("AddPatientNote")]
        public IActionResult AddNote(PatientNotesDTO patientNotesDto)
        {
         string status= _noteService.AddPatientNote(patientNotesDto);
          _unitOfWork.CommitChanges();

          return Ok(new {Status=status});
        }

        [HttpGet("GetPatientsNotesByDodId/{doctorId}")]
        public IActionResult GetNote(string doctorId)
        {
          List<GetPatientNoteData> PatientNotes = _noteService.GetPateintNotes(doctorId);

          return Ok(PatientNotes);
        }

        [HttpPut("ConfirmAccount")]
        public IActionResult ConfirmAccount(SubscribeDto subscribeDto)
        {
            if (ModelState.IsValid)
            {
                string Status = _patientService.Confirm(subscribeDto);
                if (Status != "")
                {
                    _unitOfWork.CommitChanges();

                    return Ok(new
                    {
                        msg=Status
                    });
                }
                else
                {
                    return Ok(new
                    {
                        msg="NotFound"
                    });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }


        }

        [HttpPut("RejectAccount")]
        public IActionResult RejectAccount(SubscribeDto subscribeDto)
        {
            if (ModelState.IsValid)
            {
                string Status = _patientService.Reject(subscribeDto);
                _unitOfWork.CommitChanges();

                return Ok(new
                {
                    msg=Status
                });
            }
            else
            {
                return BadRequest(ModelState);
            }


        }

        [HttpPost("Subscribtion")]
        public IActionResult Subscribtion(SubscribeDto subscribeDto)
        {
            if (ModelState.IsValid)
            {
               string status= _patientService.Subscription(subscribeDto);
                _unitOfWork.CommitChanges();

                return Ok(new
                {
                  msg= status
                });
            }
            else
            {
                return BadRequest(ModelState);
            }


        }


        [HttpGet("patientDataDTO/{patientid}")]
        public IActionResult GetPatientDtoById(string patientid)
        {
            UserDataDTO patient = _patientService.GetPatientDataDTO(patientid);

            return Ok(patient);

        }

        [HttpGet("GetPatientsByDoctorIdWithStatusConfirmed")]
        public IActionResult GetPatientsByDoctorIdWithStatusConfirmed(string Doctorid)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<PatientDTO> patients = _patientService.GetPatientsByDoctorIdWithStatusConfirmed(Doctorid);
                return Ok(patients);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("GetPatientsByDoctorIdWithStatusWaiting")]
        public IActionResult GetPatientsByDoctorIdWithStatusWaiting(string Doctorid)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<PatientDTO> patients = _patientService.GetPatientsByDoctorIdWithStatusWaiting(Doctorid);
                return Ok(patients);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("EditPatientData")]
        public IActionResult EditPatientData([FromForm] EditPatientDto patientData, [FromForm] params string[] properties)
        {
            List<string> Properties = properties[0].Split(',').ToList();
            string[] propertiesArray = Properties.ToArray();
            if (ModelState.IsValid)
            {
                _patientService.EditPatientData(patientData, propertiesArray);
                _unitOfWork.CommitChanges();
                return Ok(new
                {
                    msg="done"
                });
            }


            else
            {
                return BadRequest(ModelState);
            }

        }


        [HttpGet("GetIFPatientInSubscription/{patientid}")]
        public IActionResult GetIFPatientInSubscription(string patientid)
        {
            string Status = _patientService.GetStatus(patientid);

            return Ok(new
            {
                msg = Status
            });

        }

        [HttpGet("GetIFPatientIsComfirmed/{patientid}")]
        public IActionResult GetIFPatientIsComfirmed(string patientid)
        {
            DoctorPatientBridge doctorPatientBridge = _patientService.GetStatusIfComfirmed(patientid);
            if (doctorPatientBridge != null)
            {
                return Ok(new
                {
                    msg = doctorPatientBridge.Status.ToString(),
                    doctorid = doctorPatientBridge.DoctorID
                });
            }

            return Ok(new
            {
                msg = "NotComfirmed",
            });



        }

        [HttpPut("CanceledSubscription")]
        public IActionResult CanceledSubscription(SubscribeDto subscribeDto)
        {
            if (ModelState.IsValid)
            {
                string Status = _patientService.CancelStatus(subscribeDto);
                
                _unitOfWork.CommitChanges();

                return Ok(new
                {
                    msg = Status
                });
            }
            else
            {
                return BadRequest(ModelState);
            }


        }

    [HttpPost("ChangePassowrd")]
    public async Task<IActionResult> ChangePassowrd(DoctorNewPasswordDTO newPassowrd)
    {

      if (ModelState.IsValid)
      {
        Patient patient = _patientService.GetById(newPassowrd.DoctorId);

        if (patient != null)
        {
          PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
          var newPasswordHash = passwordHasher.HashPassword(patient.ApplicationUser, newPassowrd.Password);

          patient.ApplicationUser.PasswordHash = newPasswordHash;
          var result = await _userManager.UpdateAsync(patient.ApplicationUser);

          RegisterDto registerDto = new RegisterDto();
          _unitOfWork.CommitChanges();

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

    [HttpGet("UpdatePatientNoteStatus/{NoteId}")]
    public IActionResult UpdatePatientNoteStatus(int NoteId)
    {
      PatientNotes patientNotes = _noteService.UpdateNoteStatus(NoteId);
      _unitOfWork.CommitChanges();
      return Ok(patientNotes);
    }


  }
}
