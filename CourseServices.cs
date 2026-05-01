using System;
using System.Collections.Generic;
using System.Linq;

namespace cs330_proj1
{
    public class CourseServices : ICourseServices
    {
        private readonly MySQLCourseRepository repo;

        public CourseServices(string connectionString)
        {
            repo = new MySQLCourseRepository(connectionString);
        }

        public List<Course> getCourses()
        {
            return repo.GetAllCourses().ToList();
        }

        public Course getCourseByName(string courseName)
        {
            return repo.GetCourseByName(courseName);
        }

        public List<Course> getCoursesByDept(string dept)
        {
            return repo.GetAllCourses()
                .Where(c => c.Name.StartsWith(dept, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Course addCourse(Course newCourse)
        {
            if (newCourse == null || String.IsNullOrWhiteSpace(newCourse.Name))
                return null;
            if (repo.GetCourseByName(newCourse.Name) != null)
                return null;
            return repo.InsertCourse(newCourse);
        }

        public Course updateCourse(string courseName, Course updatedCourse)
        {
            if (updatedCourse == null) return null;
            bool updated = repo.UpdateCourseByName(courseName, updatedCourse);
            if (!updated) return null;
            return repo.GetCourseByName(courseName);
        }

        public bool deleteCourse(string courseName)
        {
            return repo.DeleteCourseByName(courseName);
        }

        public List<CoreGoal> getCoreGoalsByCourseName(string courseName)
        {
            var course = repo.GetCourseByName(courseName);
            if (course == null) return null;
            return repo.GetAllCoreGoals()
                .Where(cg => repo.GetCoursesForCoreGoalById(cg.Id)
                    .Any(c => c.Name.Equals(courseName, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public List<CourseOffering> getCourseOfferingsByCourseAndSemester(string courseName, string semester)
        {
            // offerings not in MySQL scope for this sprint — uses in-memory fallback
            return new List<CourseOffering>();
        }

        public List<CourseOffering> getOfferingsByGoalIdAndSemester(string theGoalId, string semester)
        {
            return new List<CourseOffering>();
        }

        public List<CourseOffering> getCourseOfferingsBySemester(string semester)
        {
            return new List<CourseOffering>();
        }

        public List<CourseOffering> getCourseOfferingsBySemesterAndDept(string semester, string dept)
        {
            return new List<CourseOffering>();
        }

        public List<Course> getCoursesByGoalId(string theGoalId)
        {
            return repo.GetCoursesForCoreGoalById(theGoalId).ToList();
        }

        public List<Course> getCoursesByGoalIds(string goalId1, string goalId2)
        {
            var list1 = repo.GetCoursesForCoreGoalById(goalId1).Select(c => c.Name).ToHashSet();
            return repo.GetCoursesForCoreGoalById(goalId2)
                .Where(c => list1.Contains(c.Name))
                .ToList();
        }

        public List<CoreGoal> getCoreGoalsThatAreNotCoveredBySemester(string semester)
        {
            return new List<CoreGoal>();
        }

        // CoreGoal service methods used by CoreGoalsController

        public IEnumerable<CoreGoal> getAllCoreGoals()
        {
            return repo.GetAllCoreGoals();
        }

        public CoreGoal getCoreGoalById(string id)
        {
            return repo.GetCoreGoalById(id);
        }

        public CoreGoal getCoreGoalWithCoursesById(string id)
        {
            return repo.GetCoreGoalWithCoursesById(id);
        }

        public IEnumerable<Course> getCoursesForCoreGoalById(string id)
        {
            return repo.GetCoursesForCoreGoalById(id);
        }

        public CoreGoal insertCoreGoal(CoreGoal newGoal)
        {
            if (newGoal == null || String.IsNullOrWhiteSpace(newGoal.Id))
                return null;
            if (repo.GetCoreGoalById(newGoal.Id) != null)
                return null;
            return repo.InsertCoreGoal(newGoal);
        }

        public bool updateCoreGoal(string id, CoreGoal modified)
        {
            return repo.UpdateCoreGoal(id, modified);
        }

        public bool addCourseToCoreGoal(string id, Course course)
        {
            return repo.AddCourseToCoreGoal(id, course);
        }

        public bool deleteCoreGoal(string id)
        {
            return repo.DeleteCoreGoal(id);
        }
    }
}
