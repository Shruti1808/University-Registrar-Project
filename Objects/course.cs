using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace University
{
    public class Course
    {
        private int _id;
        private string _title;
        private int _dept_id;
        private string _courseNumber;

        public Course(string title,  int dept_id, string courseNumber, int id = 0)
        {
            _id = id;
            _title = title;
            _dept_id = dept_id;
            _courseNumber = courseNumber;
        }

        public override bool Equals(System.Object otherCourse)
        {
            if (!(otherCourse is Course))
            {
                return false;
            }
            else
            {
                Course newCourse = (Course) otherCourse;
                bool idEquality = (this.GetId() == newCourse.GetId());
                bool titleEquality = (this.GetTitle()== newCourse.GetTitle());
                bool courseNumberEquality = (this.GetCourseNumber() == newCourse.GetCourseNumber());
                bool deptIdEquality = (this.GetDeptId() == newCourse.GetDeptId());
                return (idEquality && titleEquality && courseNumberEquality && deptIdEquality);
            }
        }

        public static List<Course> GetAll()
        {
            List<Course> CourseList = new List<Course> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM courses;", conn);


            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int courseID = rdr.GetInt32(0);
                string courseTitle = rdr.GetString(1);
                int deptID = rdr.GetInt32(2);
                string courseNumber = rdr.GetString(3);
                Course newCourse = new Course(courseTitle, deptID, courseNumber, courseID);
                CourseList.Add(newCourse);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return CourseList;
        }


        public void Save()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO courses(title, course_number, department_id) OUTPUT INSERTED.id VALUES (@Title, @CourseNumber, @DeptId);", connection);

            SqlParameter studTitleParam = new SqlParameter("@Title", this.GetTitle());
            SqlParameter courseNumberParam = new SqlParameter("@CourseNumber", this.GetCourseNumber());
            SqlParameter deptidParam = new SqlParameter("@DeptId", this.GetDeptId());

            cmd.Parameters.Add(studTitleParam);
            cmd.Parameters.Add(courseNumberParam);
            cmd.Parameters.Add(deptidParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
                this._id = rdr.GetInt32(0);
            }
            if (rdr != null)
            {
                rdr.Close();
            }
            if(connection != null)
            {
                connection.Close();
            }
        }

        public static Course Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId", conn);

            SqlParameter idParam = new SqlParameter();
            idParam.ParameterName = "@CourseId";
            idParam.Value = id.ToString();
            cmd.Parameters.Add(idParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundCourseId = 0;
            string foundTitle = null;
            string foundCourseNumber = null;
            int foundDeptId = 0;

            while (rdr.Read())
            {
                foundCourseId = rdr.GetInt32(0);
                foundTitle = rdr.GetString(1);
                foundDeptId = rdr.GetInt32(2);
                foundCourseNumber = rdr.GetString(3);
            }
            Course foundCourse = new Course(foundTitle, foundDeptId, foundCourseNumber, foundCourseId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundCourse;
        }

        public void AddStudent(Student newStudent)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO major_track (course_id, student_id) VALUES (@CourseId, @StudentId);", conn);

            SqlParameter idParam = new SqlParameter("@CourseId", this.GetId());
            SqlParameter studentIdParam = new SqlParameter("@StudentId", newStudent.GetId());

            cmd.Parameters.Add(idParam);
            cmd.Parameters.Add(studentIdParam);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }
        }

        public List<Student> GetStudents()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT students.* FROM courses join major_track ON (courses.id = major_track.course_id) JOIN students ON (major_track.student_id = students.id) WHERE courses.id = @CourseId", conn);

            SqlParameter courseIdParam = new SqlParameter("@CourseId", this.GetId().ToString());
            cmd.Parameters.Add(courseIdParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<Student> StudentList = new List<Student> {};

                while (rdr.Read())
                {
                    int matchStudentId = rdr.GetInt32(0);
                    string name = rdr.GetString(1);
                    string enrollDate = rdr.GetString(2);
                    int deptId = rdr.GetInt32(3);
                    Student newStudent = new Student(name, enrollDate, deptId, matchStudentId);
                    StudentList.Add(newStudent);
                }

                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
            return StudentList;
        }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM courses;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public int GetId()
        {
            return _id;
        }

        public string GetTitle()
        {
            return _title;
        }
        public string GetCourseNumber()
        {
            return _courseNumber;
        }

        public int GetDeptId()
        {
            return _dept_id;
        }
    }
}
