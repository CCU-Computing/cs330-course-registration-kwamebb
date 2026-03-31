using System;
using System.Collections.Generic;

namespace cs330_proj1
{
    public interface ICourseServices
    {
        List<Course> getCourses();
        Course getCourseByName(String courseName);
        List<Course> getCoursesByDept(String dept);
        Course addCourse(Course newCourse);
        Course updateCourse(String courseName, Course updatedCourse);
        bool deleteCourse(String courseName);
        List<CoreGoal> getCoreGoalsByCourseName(String courseName);
        List<CourseOffering> getCourseOfferingsByCourseAndSemester(String courseName, String semester);

        // Existing service methods (kept for compatibility)
        List<CourseOffering> getOfferingsByGoalIdAndSemester(String theGoalId, String semester);
        List<CourseOffering> getCourseOfferingsBySemester(String semester);
        List<CourseOffering> getCourseOfferingsBySemesterAndDept(String semester, String dept);
        List<Course> getCoursesByGoalId(String theGoalId);
        List<Course> getCoursesByGoalIds(String goalId1, String goalId2);
        List<CoreGoal> getCoreGoalsThatAreNotCoveredBySemester(String semester);
    }
}
