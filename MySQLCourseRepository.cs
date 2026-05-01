using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using MySqlConnector;

namespace cs330_proj1
{
    public class MySQLCourseRepository
    {
        private readonly string _connectionString;

        public MySQLCourseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private MySqlConnection GetConnection() => new MySqlConnection(_connectionString);

        // ---------------------------------------------------------------
        // Courses CRRUD
        // ---------------------------------------------------------------

        public IEnumerable<Course> GetAllCourses()
        {
            using var conn = GetConnection();
            return conn.Query<Course>("SELECT * FROM Courses");
        }

        public Course GetCourseByName(string name)
        {
            using var conn = GetConnection();
            return conn.QueryFirstOrDefault<Course>(
                "SELECT * FROM Courses WHERE Name = @Name", new { Name = name });
        }

        public Course InsertCourse(Course course)
        {
            using var conn = GetConnection();
            int rows = conn.Execute(
                "INSERT INTO Courses (Name, Title, Credits, Description) VALUES (@Name, @Title, @Credits, @Description)",
                course);
            return rows > 0 ? course : null;
        }

        public bool UpdateCourseByName(string name, Course updated)
        {
            using var conn = GetConnection();
            int rows = conn.Execute(
                "UPDATE Courses SET Title=@Title, Credits=@Credits, Description=@Description WHERE Name=@Name",
                new { updated.Title, updated.Credits, updated.Description, Name = name });
            return rows > 0;
        }

        public bool DeleteCourseByName(string name)
        {
            using var conn = GetConnection();
            conn.Execute("DELETE FROM CoreGoalCourses WHERE CourseName=@Name", new { Name = name });
            int rows = conn.Execute("DELETE FROM Courses WHERE Name=@Name", new { Name = name });
            return rows > 0;
        }

        // ---------------------------------------------------------------
        // CoreGoals CRRUD
        // ---------------------------------------------------------------

        public IEnumerable<CoreGoal> GetAllCoreGoals()
        {
            using var conn = GetConnection();
            var goals = conn.Query<CoreGoal>("SELECT * FROM CoreGoals").ToList();
            foreach (var g in goals)
                g.Courses = new List<Course>();
            return goals;
        }

        public CoreGoal GetCoreGoalById(string id)
        {
            using var conn = GetConnection();
            var goal = conn.QueryFirstOrDefault<CoreGoal>(
                "SELECT * FROM CoreGoals WHERE Id=@Id", new { Id = id });
            if (goal != null)
                goal.Courses = new List<Course>();
            return goal;
        }

        public CoreGoal GetCoreGoalWithCoursesById(string id)
        {
            using var conn = GetConnection();
            var goal = conn.QueryFirstOrDefault<CoreGoal>(
                "SELECT * FROM CoreGoals WHERE Id=@Id", new { Id = id });
            if (goal == null) return null;

            goal.Courses = conn.Query<Course>(
                @"SELECT c.* FROM Courses c
                  JOIN CoreGoalCourses cgc ON c.Name = cgc.CourseName
                  WHERE cgc.GoalId = @Id", new { Id = id }).ToList();
            return goal;
        }

        public IEnumerable<Course> GetCoursesForCoreGoalById(string id)
        {
            using var conn = GetConnection();
            return conn.Query<Course>(
                @"SELECT c.* FROM Courses c
                  JOIN CoreGoalCourses cgc ON c.Name = cgc.CourseName
                  WHERE cgc.GoalId = @Id", new { Id = id });
        }

        public CoreGoal InsertCoreGoal(CoreGoal newGoal)
        {
            using var conn = GetConnection();
            int rows = conn.Execute(
                "INSERT INTO CoreGoals (Id, Name, Description) VALUES (@Id, @Name, @Description)",
                new { newGoal.Id, newGoal.Name, newGoal.Description });
            if (rows == 0) return null;
            newGoal.Courses = new List<Course>();
            return newGoal;
        }

        public bool UpdateCoreGoal(string id, CoreGoal modified)
        {
            using var conn = GetConnection();
            int rows = conn.Execute(
                "UPDATE CoreGoals SET Name=@Name, Description=@Description WHERE Id=@Id",
                new { modified.Name, modified.Description, Id = id });
            return rows > 0;
        }

        public bool AddCourseToCoreGoal(string id, Course course)
        {
            using var conn = GetConnection();
            int rows = conn.Execute(
                "INSERT IGNORE INTO CoreGoalCourses (GoalId, CourseName) VALUES (@GoalId, @CourseName)",
                new { GoalId = id, CourseName = course.Name });
            return rows > 0;
        }

        public bool DeleteCoreGoal(string id)
        {
            using var conn = GetConnection();
            conn.Execute("DELETE FROM CoreGoalCourses WHERE GoalId=@Id", new { Id = id });
            int rows = conn.Execute("DELETE FROM CoreGoals WHERE Id=@Id", new { Id = id });
            return rows > 0;
        }
    }
}
