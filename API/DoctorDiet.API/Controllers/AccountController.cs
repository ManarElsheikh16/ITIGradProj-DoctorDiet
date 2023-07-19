using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using DoctorDiet.DTO;
using DoctorDiet.Models;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.Services;
using System;
using System.Security.Permissions;
using DoctorDiet.Dto;

namespace DoctorDiet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private IMapper _mapper;
        private DoctorService _doctorService;
        private AdminService _adminService;
        PatientService _patientService;
        IUnitOfWork _unitOfWork;
        AccountService _accountService;
        public AccountController(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,IMapper mapper, DoctorService doctorService,
            AdminService adminService,IUnitOfWork unitOfWork, PatientService patientService,AccountService accountService)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            _mapper=mapper;
            _unitOfWork = unitOfWork;
            _patientService = patientService;
            _doctorService = doctorService;
            _adminService = adminService;
            _accountService= accountService;

        }
        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
           string Message = await _accountService.ForgetPassword(forgetPasswordDTO);
            _unitOfWork.LoadData();
            _unitOfWork.CommitChanges();
            return Ok(new
            {
                msg = Message
            }) ;
        }

        [HttpGet("GetQuestionByUserName")]
        public async Task<IActionResult> GetQuestionByUserName(string UserName)
        {
            string Message = _accountService.GetQuestionByUserName(UserName);
            return Ok(new {Msg =Message });
        }

        [HttpPost("PatientRegister")]
        public async Task<IActionResult> PatientRegister([FromForm] RegisterPatientDto registerPatientDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser ApplicationUser = _mapper.Map<ApplicationUser>(registerPatientDto);

                if (registerPatientDto.ProfileImage != null)
                {
                  using var dataStream = new MemoryStream();
                  registerPatientDto.ProfileImage.CopyTo(dataStream);

                  ApplicationUser.ProfileImage = dataStream.ToArray();
                }
                IdentityResult result = await userManager.CreateAsync(ApplicationUser, registerPatientDto.Password);
                RegisterDto registerDto = new RegisterDto();

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(ApplicationUser, "Patient");

                    registerPatientDto.PatientId = ApplicationUser.Id;

                    double BMR = 0.0;

                    int MaxHisActivityRate = 0;
                    int MinHisActivityRate = 0;

                    double MinCals = 0.0;
                    double MaxCals = 0.0;

                    if (registerPatientDto.Gender == "Male")
                    {
                        BMR = 24 * 1 * registerPatientDto.Weight;

                        if (registerPatientDto.ActivityRates.Contains("veryHigh"))
                        {
                            MaxHisActivityRate = 120;
                            MinHisActivityRate = 90;
                        }

                        else if (registerPatientDto.ActivityRates.Contains("high"))
                        {
                            MaxHisActivityRate = 80;
                            MinHisActivityRate = 65;
                        }

                        else if (registerPatientDto.ActivityRates.Contains("regular"))
                        {
                            MaxHisActivityRate = 70;
                            MinHisActivityRate = 50;
                        }

                        else if (registerPatientDto.ActivityRates.Contains("low"))
                        {
                            MaxHisActivityRate = 40;
                            MinHisActivityRate = 25;
                        }


                    }

                    else if (registerPatientDto.Gender == "Female")
                    {
                        BMR = 24 * 0.9 * registerPatientDto.Weight;

                        if (registerPatientDto.ActivityRates.Contains("veryHigh"))
                        {
                            MaxHisActivityRate = 100;
                            MinHisActivityRate = 80;
                        }

                        else if (registerPatientDto.ActivityRates.Contains("high"))
                        {
                            MaxHisActivityRate = 70;
                            MinHisActivityRate = 50;
                        }

                        else if (registerPatientDto.ActivityRates.Contains("regular"))
                        {
                            MaxHisActivityRate = 60;
                            MinHisActivityRate = 40;
                        }

                        else if (registerPatientDto.ActivityRates.Contains("low"))
                        {
                            MaxHisActivityRate = 35;
                            MinHisActivityRate = 25;
                        }
                    }

                    MinCals = BMR * MinHisActivityRate / 100;
                    MaxCals = BMR * MaxHisActivityRate / 100;

                    registerPatientDto.MinCalories = (int)(MinCals + MaxCals);

                    registerPatientDto.MaxCalories = (int)(MaxCals + MaxCals);

                    _patientService.AddPatient(registerPatientDto);
                    _unitOfWork.CommitChanges();

                    registerDto.Message = "Success";
                    return Ok(registerDto);
                }
                else
                    registerDto.Message = "Failed";
                return Ok(new {msg= "DuplicateUserName" });
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("DoctorRegister")]
        public async Task<IActionResult> DoctorRegister([FromForm] RegisterDoctorDto registerDoctorDto)
        {
            if (ModelState.IsValid)
            {
                using var dataStream = new MemoryStream();
                registerDoctorDto.ProfileImage.CopyTo(dataStream);

                ApplicationUser ApplicationUser = _mapper.Map<ApplicationUser>(registerDoctorDto);
                ApplicationUser.ProfileImage = dataStream.ToArray();

                IdentityResult result = await userManager.CreateAsync(ApplicationUser, registerDoctorDto.Password);
                RegisterDto registerDto = new RegisterDto();

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(ApplicationUser, "Doctor");

                    _doctorService.AddDoctor(ApplicationUser.Id, registerDoctorDto);
                    _unitOfWork.CommitChanges();

                    registerDto.Message = "Success";
                    return Ok(registerDto);
                }
                else
                    registerDto.Message = "Failed";
                return BadRequest(registerDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = await userManager.FindByNameAsync(userDto.UserName);//.FindByNameAsync(userDto.UserName);
                LoginDto loginDto = new LoginDto();
                if (applicationUser != null && await userManager.CheckPasswordAsync(applicationUser, userDto.Password))
                {

                    var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
                    SigningCredentials credentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256);

                    List<Claim> myClaims = new List<Claim>();

                    myClaims.Add(new Claim(ClaimTypes.NameIdentifier, applicationUser.Id));
                    myClaims.Add(new Claim(ClaimTypes.Name, applicationUser.UserName));
                    myClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                    if (await userManager.IsInRoleAsync(applicationUser, "Patient"))
                    {
                        myClaims.Add(new Claim(ClaimTypes.Role, "Patient"));
                    }
                    else if (await userManager.IsInRoleAsync(applicationUser, "Admin"))
                    {
                        myClaims.Add(new Claim(ClaimTypes.Role, "Admin"));
                    }
                    else if (await userManager.IsInRoleAsync(applicationUser, "Doctor"))
                    {
                        myClaims.Add(new Claim(ClaimTypes.Role, "Doctor"));
                    }
                    else
                    {
                        myClaims.Add(new Claim(ClaimTypes.Role, "NoRole"));
                    }

                        JwtSecurityToken MyToken = new JwtSecurityToken(
                        issuer: configuration["JWT:ValidIssuer"],
                        audience: configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(6),
                        claims: myClaims,


                        signingCredentials: credentials
                        );
                    loginDto.Message = "Success";

                    return Ok(
                        new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(MyToken),
                            expiration = MyToken.ValidTo,
                            Messege = "Success"
                        });

                }
                else
                {
                    loginDto.Message = "Invalid UserName";
                    return BadRequest(loginDto);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }

}

