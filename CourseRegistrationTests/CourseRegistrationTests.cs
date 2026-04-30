using System;
using System.Collections.Generic;
using Xunit;
using cs330_proj1;

namespace CourseRegistrationTests
{
    public class CourseRegistrationTests
    {
        private CourseServices services;

        public CourseRegistrationTests()
        {
            services = new CourseServices();
        }

        // ---------------------------------------------------------------
        // User Story 2: As a student, I want to see all available courses
        // so that I know what my options are.
        // ---------------------------------------------------------------

        [Fact]
        public void GetCourses_ReturnsAllFiveCourses()
        {
            List<Course> courses = services.getCourses();
            Assert.Equal(5, courses.Count);
        }

        [Fact]
        public void GetCourses_ContainsCSCI201()
        {
            List<Course> courses = services.getCourses();
            bool found = false;
            foreach (Course c in courses)
            {
                if (c.Name == "CSCI 201") { found = true; break; }
            }
            Assert.True(found);
        }

        [Fact]
        public void GetCourses_ContainsARTD201()
        {
            List<Course> courses = services.getCourses();
            bool found = false;
            foreach (Course c in courses)
            {
                if (c.Name == "ARTD 201") { found = true; break; }
            }
            Assert.True(found);
        }

        [Fact]
        public void GetCourses_ReturnsNonEmptyList()
        {
            List<Course> courses = services.getCourses();
            Assert.NotEmpty(courses);
        }

        // ---------------------------------------------------------------
        // User Story 3: As a student, I want to see all course offerings
        // by semester, so that I can choose from what's available.
        // ---------------------------------------------------------------

        [Fact]
        public void GetCourseOfferingsBySemester_Spring2021_ReturnsTwoOfferings()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemester("Spring 2021");
            Assert.Equal(2, offerings.Count);
        }

        [Fact]
        public void GetCourseOfferingsBySemester_Spring2022_ReturnsOneOffering()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemester("Spring 2022");
            Assert.Equal(1, offerings.Count);
        }

        [Fact]
        public void GetCourseOfferingsBySemester_Fall2020_ReturnsTwoOfferings()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemester("Fall 2020");
            Assert.Equal(2, offerings.Count);
        }

        [Fact]
        public void GetCourseOfferingsBySemester_InvalidSemester_ReturnsEmptyList()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemester("Summer 1999");
            Assert.Empty(offerings);
        }

        [Fact]
        public void GetCourseOfferingsBySemester_Spring2021_ContainsARTD201()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemester("Spring 2021");
            bool found = false;
            foreach (CourseOffering co in offerings)
            {
                if (co.TheCourse.Name == "ARTD 201") { found = true; break; }
            }
            Assert.True(found);
        }

        // ---------------------------------------------------------------
        // User Story 4: As a student, I want to see all course offerings
        // by semester and department so that I can choose major courses.
        // ---------------------------------------------------------------

        [Fact]
        public void GetCourseOfferingsBySemesterAndDept_Fall2020_CSCI_ReturnsOne()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemesterAndDept("Fall 2020", "CSCI");
            Assert.Equal(1, offerings.Count);
        }

        [Fact]
        public void GetCourseOfferingsBySemesterAndDept_Fall2020_CSCI_ContainsCSCI201()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemesterAndDept("Fall 2020", "CSCI");
            Assert.Equal("CSCI 201", offerings[0].TheCourse.Name);
        }

        [Fact]
        public void GetCourseOfferingsBySemesterAndDept_Spring2021_ARTS_ReturnsEmpty()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemesterAndDept("Spring 2021", "ARTS");
            Assert.Empty(offerings);
        }

        [Fact]
        public void GetCourseOfferingsBySemesterAndDept_Fall2020_ENGL_ReturnsOne()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemesterAndDept("Fall 2020", "ENGL");
            Assert.Equal(1, offerings.Count);
        }

        [Fact]
        public void GetCourseOfferingsBySemesterAndDept_Spring2022_ARTS_ReturnsOne()
        {
            List<CourseOffering> offerings = services.getCourseOfferingsBySemesterAndDept("Spring 2022", "ARTS");
            Assert.Equal(1, offerings.Count);
        }
    }
}
