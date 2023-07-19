using DoctorDiet.Dto;
using DoctorDiet.Models;
using DoctorDiet.Repository.UnitOfWork;
using DoctorDiet.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;


namespace DoctorDiet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : Controller
    {
        private readonly PlanService _planService;
        private readonly IUnitOfWork _unitOfWork;

        public PlanController(PlanService planService, IUnitOfWork unitOfWork)
        {
            _planService = planService;
            _unitOfWork = unitOfWork;
        }

        

        [HttpGet("GetAllPlansByDoctotId")]
        public IActionResult GetAllPlansByDoctotId(string doctorID)
        {
            List<PlanDataDTO> plans = _planService.GetPlanByDoctorId(doctorID);

            return Ok(plans);
        }


        [HttpGet("GetPlanById")]
        public IActionResult GetPlanById(int id)
        {
          PlanDataDTO planData=  _planService.GetPlanById(id);
            return Ok(planData);
        }
        [HttpGet("GetDaysByPlanId")]
        public IActionResult GetDaysByPlanId(int planId)
        {
            List<Day> days = _planService.GetDaysDTOByPlanId(planId);

            return Ok(days);
        }

        [HttpGet("GetMealsByDayId")]
        public IActionResult GetMealsByDayId(int dayId)
        {
            List<MealDTO> meals = _planService.GetMealsByDayId(dayId);

            return Ok(meals);
        }

        [HttpPost]
        public IActionResult AddPlan(AddPlanDTO plan)
        {
            if (ModelState.IsValid)
            {
              _planService.AddPlan(plan);
              _unitOfWork.CommitChanges();
              return Ok(new
              {
                msg = "added"
              });
            }
            return BadRequest();
        }

        [HttpDelete("DeletePlan{id}")]
        public IActionResult DeletePlan(int id)
        {
            _planService.DeletePlan(id);
            _unitOfWork.CommitChanges();
            return Ok(new
            {
              msg="deleted"
            });
        }

        [HttpGet("GetMealById{id}")]
        public IActionResult GetMealById(int id)
        {
            MealDTO meal = _planService.GetMealById(id);
            if (meal == null)
                return NotFound();

            
            return Ok(meal);
        }

        [HttpPut("UpdateMeal")]
        public IActionResult UpdateMealInPlan([FromForm] UpdateMealDTO mealPlanDTO, [FromForm] params string[] properties)
        {
            List<string> Properties = properties[0].Split(',').ToList();
            string[] propertiesArray = Properties.ToArray();

            if (ModelState.IsValid)
            {
                _planService.UpdateMealInPlan(mealPlanDTO, propertiesArray);
                _unitOfWork.CommitChanges();
                return Ok();
            }
            return BadRequest();

        }
        [HttpPut("UpdatePlan")]
        public IActionResult UpdatePlan([FromForm]UpdatePlanDTO updatePlanDTO, [FromForm] params string[] properties)
        {
            List<string> Properties = properties[0].Split(',').ToList();
            string[] propertiesArray = Properties.ToArray();

            if (ModelState.IsValid==true)
            {
                Plan plan = _planService.UpdatePlan(updatePlanDTO, propertiesArray);
                _unitOfWork.CommitChanges();

                return Ok(plan);
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

    }
}
