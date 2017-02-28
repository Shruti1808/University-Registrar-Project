using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace University
{
    public class Student
    {
        private int _id;
        private string _name;
        private string _date;
        private int _dept_id;

        public Student(string name, string date, int dept_id, int id = 0)
        {
            _id = id;
            _name = name;
            _date = date;
            _dept_id = dept_id;
        }

        public override bool Equals(System.Object otherStudent)
        {
            if (!(otherStudent is Student))
            {
                return false;
            }
            else
            {
                Student newStudent = (Student) otherStudent;
                bool idEquality = (this.GetId() == newStudent.GetId());
                bool nameEquality = (this.GetName()== newStudent.GetName());
                bool dateEquality = (this.GetDate() == newStudent.GetDate());
                bool deptIdEquality = (this.GetDeptId() == newStudent.GetDeptId());
                return (idEquality && nameEquality && dateEquality && deptIdEquality);
            }
        }

        public static List<Student> GetAll()
        {
            List<Student> StudentList = new List<Student> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students;", conn);


            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int studentID = rdr.GetInt32(0);
                string studentName = rdr.GetString(1);
                string enrollDate = rdr.GetString(2);
                int deptID = rdr.GetInt32(3);
                Student newStudent = new Student(studentName, enrollDate, deptID, studentID);
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


        public void Save()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("INSERT into students(name, date, department_id) OUTPUT INSERTED.id VALUES (@Name, @Date, @DeptId);", connection);

            SqlParameter studNameParam = new SqlParameter("@Name", this.GetName());
            SqlParameter dateParam = new SqlParameter("@Date", this.GetDate());
            SqlParameter deptidParam = new SqlParameter("@DeptId", this.GetDeptId());

            cmd.Parameters.Add(studNameParam);
            cmd.Parameters.Add(dateParam);
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

        public static Student Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM students WHERE id = @StudentId", conn);

            SqlParameter idParam = new SqlParameter();
            idParam.ParameterName = "@StudentId";
            idParam.Value = id.ToString();
            cmd.Parameters.Add(idParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundStudentId = 0;
            string foundName = null;
            string foundDate = null;
            int foundDeptId = 0;

            while (rdr.Read())
            {
                foundStudentId = rdr.GetInt32(0);
                foundName = rdr.GetString(1);
                foundDate = rdr.GetString(2);
                foundDeptId = rdr.GetInt32(3);
            }
            Student foundStudent = new Student(foundName, foundDate, foundDeptId, foundStudentId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundStudent;
        }


        public void AddCourse(Course newCourse)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd  = new SqlCommand("INSERT INTO major_track (student_id, course_id) VALUES (@StudentId, @CourseId);",conn);

            SqlParameter studentParameter = new SqlParameter("@StudentId", this.GetId());
            SqlParameter courseParameter = new SqlParameter("@CourseId", newCourse.GetId());

            cmd.Parameters.Add(studentParameter);
            cmd.Parameters.Add(courseParameter);

            cmd.ExecuteNonQuery();

            if (conn != null)
            {
                conn.Close();
            }

        }

        public List<Course> GetCourses()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT  course_id FROM major_track WHERE student_id = @StudentId;",conn);

            SqlParameter idParam = new SqlParameter("@StudentId", this.GetId());
            cmd.Parameters.Add(idParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            List<int> CourseIds = new List<int>{};

            while(rdr.Read())
            {
                int CourseId = rdr.GetInt32(0);
                CourseIds.Add(CourseId);
            }
            if (rdr != null)
            {
                rdr.Close();
            }

            List<Course> CourseList = new List<Course>{};

            foreach (int CourseId in CourseIds)
            {
                SqlCommand courseQuery = new SqlCommand("SELECT * FROM courses WHERE id = @CourseId;", conn);
                SqlParameter courseIDParam = new SqlParameter("@CourseId", CourseId);

                courseQuery.Parameters.Add(courseIDParam);

                SqlDataReader queryReader = courseQuery.ExecuteReader();
                while(queryReader.Read())
                {
                    int matchedCourseId =  queryReader.GetInt32(0);
                    string courseTitle = queryReader.GetString(1);
                    int deptId = queryReader.GetInt32(2);
                    string courseNumber = queryReader.GetString(3);
                    Course newCourse = new Course(courseTitle, deptId, courseNumber, matchedCourseId);
                    CourseList.Add(newCourse);
                }
                if (queryReader != null)
                {
                    queryReader.Close();
                }
            }
            if (conn != null)
            {
                conn.Close();
            }
            return CourseList;
        }


        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM students;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public int GetId()
        {
            return _id;
        }

        public string GetName()
        {
            return _name;
        }
        public string GetDate()
        {
            return _date;
        }

        public int GetDeptId()
        {
            return _dept_id;
        }
    }
}
