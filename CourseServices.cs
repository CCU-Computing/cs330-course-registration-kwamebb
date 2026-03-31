using System;
using System.Collections.Generic;

namespace cs330_proj1
{
    public class CourseServices : ICourseServices
    {
        private CourseRepository repo = new CourseRepository();


        //As a student, I want to search for course offerings that meet core goals 
        // so that I can register easily for courses that meet my program requirements
         public List<CourseOffering> getOfferingsByGoalIdAndSemester(String theGoalId, String semester) {
            //use the repo to get the data from the database (data store)
            List<CoreGoal> theGoals = repo.Goals;
            List<CourseOffering> theOfferings = repo.Offerings;
            
            //Complete any other required functionality/business logic to satisfy the requirement
            CoreGoal theGoal=null;
            foreach(CoreGoal cg in theGoals) {
                if(cg.Id.Equals(theGoalId)) {
                    theGoal=cg; break;
                }
            }
            if(theGoal==null) throw new Exception("Didn't find the goal");
            //search list of courses, then for each course, search offerings
            List<CourseOffering> courseOfferingsThatMeetGoal = new List<CourseOffering>();
            
            foreach(CourseOffering c in theOfferings) {
                if(c.Semester.Equals(semester) 
                    && theGoal.Courses.Contains(c.TheCourse) ) 
                {
                    courseOfferingsThatMeetGoal.Add(c);
                }
            }
            return courseOfferingsThatMeetGoal;
        }

        
        //Add more service functions here, as needed, for the project
         
        /* As a student, I want to see all available courses so that I know what my options are */
         public List<Course> getCourses() {
            return repo.Courses;
        }

        // Get a single course by its name (e.g., "ARTD 105")
         public Course getCourseByName(String courseName) {
            foreach(Course c in repo.Courses) {
                if(c.Name.Equals(courseName, StringComparison.OrdinalIgnoreCase)) {
                    return c;
                }
            }
            return null;
        }

        // Search for courses by department prefix (e.g., "ARTS")
         public List<Course> getCoursesByDept(String dept) {
            List<Course> result = new List<Course>();
            foreach(Course c in repo.Courses) {
                if(c.Name.StartsWith(dept, StringComparison.OrdinalIgnoreCase)) {
                    result.Add(c);
                }
            }
            return result;
        }

        // Add a new course; returns null if the course already exists
         public Course addCourse(Course newCourse) {
            if(newCourse == null || String.IsNullOrWhiteSpace(newCourse.Name)) {
                return null;
            }
            if(getCourseByName(newCourse.Name) != null) {
                return null;
            }
            repo.Courses.Add(newCourse);
            return newCourse;
        }

        // Update an existing course; returns null if not found
         public Course updateCourse(String courseName, Course updatedCourse) {
            Course existing = getCourseByName(courseName);
            if(existing == null || updatedCourse == null) {
                return null;
            }
            existing.Title = updatedCourse.Title;
            existing.Credits = updatedCourse.Credits;
            existing.Description = updatedCourse.Description;
            return existing;
        }

        // Delete an existing course; returns false if not found
         public bool deleteCourse(String courseName) {
            Course existing = getCourseByName(courseName);
            if(existing == null) {
                return false;
            }
            repo.Courses.Remove(existing);

            // Remove course from any core goals that reference it
            foreach(CoreGoal cg in repo.Goals) {
                if(cg.Courses.Contains(existing)) {
                    cg.Courses.Remove(existing);
                }
            }

            // Remove course offerings for the deleted course
            for(int i = repo.Offerings.Count - 1; i >= 0; i--) {
                if(repo.Offerings[i].TheCourse == existing) {
                    repo.Offerings.RemoveAt(i);
                }
            }
            return true;
        }

        // Get core goals that include a particular course
         public List<CoreGoal> getCoreGoalsByCourseName(String courseName) {
            Course course = getCourseByName(courseName);
            if(course == null) {
                return null;
            }
            List<CoreGoal> result = new List<CoreGoal>();
            foreach(CoreGoal cg in repo.Goals) {
                if(cg.Courses.Contains(course)) {
                    result.Add(cg);
                }
            }
            return result;
        }

        // Get course offerings for a specific course and semester
         public List<CourseOffering> getCourseOfferingsByCourseAndSemester(String courseName, String semester) {
            Course course = getCourseByName(courseName);
            if(course == null) {
                return null;
            }
            List<CourseOffering> result = new List<CourseOffering>();
            foreach(CourseOffering co in repo.Offerings) {
                if(co.TheCourse == course 
                    && co.Semester.Equals(semester, StringComparison.OrdinalIgnoreCase)) {
                    result.Add(co);
                }
            }
            return result;
        }

        /* As a student, I want to see all course offerings by semester, so that I can choose from what's
           available to register for next semester */
         public List<CourseOffering> getCourseOfferingsBySemester(String semester) {
            List<CourseOffering> result = new List<CourseOffering>();
            foreach(CourseOffering co in repo.Offerings) {
                if(co.Semester.Equals(semester)) {
                    result.Add(co);
                }
            }
            return result;
        }  

        /* As a student I want to see all course offerings by semester and department so that I can 
        choose major courses to register for */
         public List<CourseOffering> getCourseOfferingsBySemesterAndDept(String semester, String dept) {
            List<CourseOffering> result = new List<CourseOffering>();
            foreach(CourseOffering co in repo.Offerings) {
                if(co.Semester.Equals(semester) 
                    && co.TheCourse.Name.StartsWith(dept)) {
                    result.Add(co);
                }
            }
            return result;
         }

        /* As a student I want to see all courses that meet a core goal, so that I can plan out
           my courses over the next few semesters and choose core courses that make sense for me */
         public List<Course> getCoursesByGoalId(String theGoalId) {
            foreach(CoreGoal cg in repo.Goals) {
                if(cg.Id.Equals(theGoalId)) {
                    return cg.Courses;
                }
            }
            throw new Exception("Didn't find the goal");
        }

        /* As a student I want to find a course that meets two different core goals, so that I can
        "feed two birds with one seed" (save time by taking one class that will fulfill two 
          requirements */
         public List<Course> getCoursesByGoalIds(String goalId1, String goalId2) {
            List<Course> coursesForGoal1 = getCoursesByGoalId(goalId1);
            List<Course> coursesForGoal2 = getCoursesByGoalId(goalId2);

            List<Course> result = new List<Course>();
            foreach(Course c in coursesForGoal1) {
                if(coursesForGoal2.Contains(c)) {
                    result.Add(c);
                }
            }
            return result;
        }

        /* As a freshman adviser, I want to see all the core goals which do not have any course offerings 
           for a given semester, so that I can work with departments to get some courses offered
           that students can take to meet those goals */

           public List<CoreGoal> getCoreGoalsThatAreNotCoveredBySemester(String semester) {
            List<CourseOffering> semesterOfferings = getCourseOfferingsBySemester(semester);

            List<CoreGoal> result = new List<CoreGoal>();
            foreach(CoreGoal cg in repo.Goals) {
                bool isCovered = false;
                foreach(Course c in cg.Courses) {
                    foreach(CourseOffering co in semesterOfferings) {
                        if(co.TheCourse == c) {
                            isCovered = true;
                            break;
                        }
                    }
                    if(isCovered) break;
                }
                if(!isCovered) {
                    result.Add(cg);
                }
            }
            return result;
        }

        
     }
}
