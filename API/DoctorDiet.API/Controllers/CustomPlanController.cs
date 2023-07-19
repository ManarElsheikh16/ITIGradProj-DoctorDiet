using DoctorDiet.Dto;
using DoctorDiet.Models;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.Services;
using Microsoft.AspNetCore.Mvc;

namespace DoctorDiet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomPlanController : Controller
    {
        private CustomPlanService _CustomPlanService;
        private DayCustomPlanService _dayCustomPlanService;
        IUnitOfWork _unitOfWork;

        public CustomPlanController(CustomPlanService planService, IUnitOfWork unitOfWork, DayCustomPlanService dayCustomPlanService)
        {
            _CustomPlanService = planService;
            _unitOfWork = unitOfWork;
            _dayCustomPlanService = dayCustomPlanService;
        }

        [HttpGet("GetDayCustomPlan")]
        public IActionResult GetDayCustomPlan(int customPlanId)
        {
            CustomDayDTO dayCustomPlan = _dayCustomPlanService.GetDayCustomPlan(customPlanId);

            if (dayCustomPlan == null)
            {
                return NotFound();
            }

            return Ok(dayCustomPlan);
        }

        [HttpPut("UpdateCustomMeal")]
        public IActionResult UpdateMealInCustomPlan( [FromForm] UpdateMealDTO mealCustomPlanDTO, [FromForm] params string[] properties)
        {
            List<string> Properties = properties[0].Split(',').ToList();
            string[] propertiesArray = Properties.ToArray();

            if (ModelState.IsValid)
            {
                _CustomPlanService.UpdateMealInCustomPlan(mealCustomPlanDTO, propertiesArray);
                _unitOfWork.CommitChanges();
                return Ok();
            }
            return BadRequest();

        }

        [HttpGet("GetDayCustomPlanByCusPlanId/{customPlanId}")]
        public IActionResult GetDayCustomPlanByCusPlanId(int customPlanId)
        {
            if (ModelState.IsValid)
            {
                List<DayCustomPlan> DaysCusPlan = _CustomPlanService.GetDayCustomPlanByCusPlanId(customPlanId);
                return Ok(DaysCusPlan);
            }
            return BadRequest();

        }

        [HttpGet("GetMealCustomPlanByCusDayId/{customDayId}")]
        public IActionResult GetMealCustomPlanByCusDayId(int customDayId)
        {
            if (ModelState.IsValid)
            {
                List<CustomMealsDTO> MealsCusPlanDto = _CustomPlanService.GetMealsByCusDayId(customDayId);
                return Ok(MealsCusPlanDto);
            }
            return BadRequest();

        }

        [HttpGet("GetCustomMealByID/{customMealId}")]
        public IActionResult GetCustomMealByID(int customMealId)
        {
            if (ModelState.IsValid)
            {
              CustomMealsDTO MealsCusPlanDto = _CustomPlanService.GetCustomMealByID(customMealId);
                return Ok(MealsCusPlanDto);
            }
            return BadRequest();

        }

        [HttpPost("CurrentCustomPlan")]
        public IActionResult CurrentCustomPlan(SubscribeDto subscribeDto)
        {
          CurrentCustomPlanDto currentCustomPlan = _CustomPlanService.GetCurrentCustomPlanId(subscribeDto);

          return Ok(currentCustomPlan);

        }


  }
}
