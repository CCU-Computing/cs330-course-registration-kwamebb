using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace cs330_proj1
{
    [ApiController]
    [Route("coregoals")]
    public class CoreGoalsController : ControllerBase
    {
        private readonly CourseServices service;

        public CoreGoalsController(ICourseServices svc)
        {
            service = (CourseServices)svc;
        }

        // GET /coregoals
        [HttpGet]
        public ActionResult<IEnumerable<CoreGoal>> GetAllCoreGoals()
        {
            return Ok(service.getAllCoreGoals());
        }

        // GET /coregoals/{id}
        [HttpGet("{id}")]
        public ActionResult<CoreGoal> GetCoreGoalById(string id)
        {
            var goal = service.getCoreGoalWithCoursesById(id);
            if (goal == null) return NotFound();
            return Ok(goal);
        }

        // GET /coregoals/{id}/courses
        [HttpGet("{id}/courses")]
        public ActionResult<IEnumerable<Course>> GetCoursesForCoreGoal(string id)
        {
            var goal = service.getCoreGoalById(id);
            if (goal == null) return NotFound();
            return Ok(service.getCoursesForCoreGoalById(id));
        }

        // POST /coregoals
        [HttpPost]
        public ActionResult<CoreGoal> InsertCoreGoal([FromBody] CoreGoal newGoal)
        {
            if (newGoal == null || String.IsNullOrWhiteSpace(newGoal.Id))
                return BadRequest("Id is required.");
            var inserted = service.insertCoreGoal(newGoal);
            if (inserted == null) return Conflict("A core goal with that Id already exists.");
            return CreatedAtAction(nameof(GetCoreGoalById), new { id = inserted.Id }, inserted);
        }

        // PUT /coregoals/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateCoreGoal(string id, [FromBody] CoreGoal modified)
        {
            if (modified == null) return BadRequest();
            bool updated = service.updateCoreGoal(id, modified);
            if (!updated) return NotFound();
            return Ok(service.getCoreGoalWithCoursesById(id));
        }

        // PUT /coregoals/{id}/courses
        [HttpPut("{id}/courses")]
        public IActionResult AddCourseToCoreGoal(string id, [FromBody] Course course)
        {
            if (course == null || String.IsNullOrWhiteSpace(course.Name))
                return BadRequest("Course name is required.");
            var goal = service.getCoreGoalById(id);
            if (goal == null) return NotFound("Core goal not found.");
            bool added = service.addCourseToCoreGoal(id, course);
            if (!added) return Conflict("Course is already linked to this core goal.");
            return Ok(service.getCoreGoalWithCoursesById(id));
        }

        // DELETE /coregoals/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCoreGoal(string id)
        {
            bool deleted = service.deleteCoreGoal(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
