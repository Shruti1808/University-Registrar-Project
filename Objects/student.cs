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

        // SqlParameters idParam = new SqlParameters();
        // idParam.ParameterName = "@StudentId";
        // idParam.Value = this.GetId();
        // cmd.Parameters.Add(idParam);
        // public override int GetHashCode()
        // {
        //     return this.GetId().GetHashCode();
        // }

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
