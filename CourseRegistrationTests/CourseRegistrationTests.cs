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
    }
}
