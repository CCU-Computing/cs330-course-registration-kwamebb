using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace cs330_proj1
{
    [ApiController]
    [Route("courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseServices service;

        public CourseController(ICourseServices service)
        {
            this.service = service;
        }

        // GET /courses
        [HttpGet]
        public ActionResult<List<Course>> GetAllCourses()
        {
            return Ok(service.getCourses());
        }

        // GET /courses/search?dept=ARTS
        [HttpGet("search")]
        public ActionResult<List<Course>> SearchCoursesByDept([FromQuery] string dept)
        {
            if (String.IsNullOrWhiteSpace(dept))
            {
                return BadRequest("dept query parameter is required.");
            }

            return Ok(service.getCoursesByDept(dept));
        }

        // GET /courses/{courseName}
        [HttpGet("{courseName}")]
        public ActionResult<Course> GetCourseByName(string courseName)
        {
            Course course = service.getCourseByName(courseName);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // POST /courses
        [HttpPost]
        public ActionResult<Course> AddCourse([FromBody] Course newCourse)
        {
            if (newCourse == null)
            {
                return BadRequest("Course body is required.");
            }

            if (String.IsNullOrWhiteSpace(newCourse.Name))
            {
                return BadRequest("Course name is required.");
            }

            Course added = service.addCourse(newCourse);
            if (added == null)
            {
                return Conflict("A course with that name already exists.");
            }

            return CreatedAtAction(nameof(GetCourseByName), new { courseName = added.Name }, added);
        }

        // PUT /courses/{courseName}
        [HttpPut("{courseName}")]
        public ActionResult<Course> UpdateCourse(string courseName, [FromBody] Course updatedCourse)
        {
            if (updatedCourse == null)
            {
                return BadRequest("Course body is required.");
            }

            if (!String.IsNullOrWhiteSpace(updatedCourse.Name)
                && !updatedCourse.Name.Equals(courseName, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Course name in body must match the route.");
            }

            Course updated = service.updateCourse(courseName, updatedCourse);
            if (updated == null)
            {
                return NotFound();
            }

            return Ok(updated);
        }

        // DELETE /courses/{courseName}
        [HttpDelete("{courseName}")]
        public IActionResult DeleteCourse(string courseName)
        {
            bool deleted = service.deleteCourse(courseName);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        // GET /courses/{courseName}/goals
        [HttpGet("{courseName}/goals")]
        public ActionResult<List<CoreGoal>> GetGoalsForCourse(string courseName)
        {
            List<CoreGoal> goals = service.getCoreGoalsByCourseName(courseName);
            if (goals == null)
            {
                return NotFound();
            }

            return Ok(goals);
        }

        // GET /courses/{courseName}/offerings?semester=Fall 2021
        [HttpGet("{courseName}/offerings")]
        public ActionResult<List<CourseOffering>> GetOfferingsForCourseBySemester(
            string courseName,
            [FromQuery] string semester)
        {
            if (String.IsNullOrWhiteSpace(semester))
            {
                return BadRequest("semester query parameter is required.");
            }

            List<CourseOffering> offerings =
                service.getCourseOfferingsByCourseAndSemester(courseName, semester);

            if (offerings == null)
            {
                return NotFound();
            }

            return Ok(offerings);
        }
    }
}
