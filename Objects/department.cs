using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace University
{
    public class Department
    {
        private int _id;
        private string _college;

        public Department(string college, int id = 0)
        {
            _id = id;
            _college = college;
        }

        public override bool Equals(System.Object otherDepartment)
        {
            if (!(otherDepartment is Department))
            {
                return false;
            }
            else
            {
                Department newDepartment = (Department) otherDepartment;
                bool idEquality = (this.GetId() == newDepartment.GetId());
                bool collegeEquality = (this.GetCollege()== newDepartment.GetCollege());
                return (idEquality && collegeEquality);
            }
        }

        public static List<Department> GetAll()
        {
            List<Department> DepartmentList = new List<Department> {};

            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM departments;", conn);


            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                int departmentId = rdr.GetInt32(0);
                string departmentCollege = rdr.GetString(1);
                Department newDepartment = new Department(departmentCollege, departmentId);
                DepartmentList.Add(newDepartment);
            }

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
            return DepartmentList;
        }


        public void Save()
        {
            SqlConnection connection = DB.Connection();
            connection.Open();

            SqlCommand cmd = new SqlCommand("INSERT INTO departments(college) OUTPUT INSERTED.id VALUES (@College);", connection);

            SqlParameter CollegeParam = new SqlParameter("@College", this.GetCollege());
            cmd.Parameters.Add(CollegeParam);


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

        public static Department Find(int id)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM departments WHERE id = @DepartmentId", conn);

            SqlParameter idParam = new SqlParameter();
            idParam.ParameterName = "@DepartmentId";
            idParam.Value = id.ToString();
            cmd.Parameters.Add(idParam);

            SqlDataReader rdr = cmd.ExecuteReader();

            int foundDepartmentId = 0;
            string foundCollege = null;
            string foundDepartmentNumber = null;
            int foundDeptId = 0;

            while (rdr.Read())
            {
                foundDepartmentId = rdr.GetInt32(0);
                foundCollege = rdr.GetString(1);
            }
            Department foundDepartment = new Department(foundCollege, foundDepartmentId);

            if (rdr != null)
            {
                rdr.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }

            return foundDepartment;
        }

        public void Update(string newCollege)
        {
            SqlConnection conn = DB.Connection();
            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE departments SET college = @NewDepartmentCollege OUTPUT INSERTED.college WHERE id = @DepartmentId;", conn);

            SqlParameter newCollegeParameter = new SqlParameter();
            newCollegeParameter.ParameterName = "@NewDepartmentCollege";
            newCollegeParameter.Value = newCollege;
            cmd.Parameters.Add(newCollegeParameter);

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@DepartmentId";
            idParameter.Value = this.GetId();
            cmd.Parameters.Add(idParameter);

            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                this._college = rdr.GetString(0);
            }
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (conn != null)
                {
                    conn.Close();
                }
        }


        public void Delete()
   {
     SqlConnection conn = DB.Connection();
     conn.Open();

     SqlCommand cmd = new SqlCommand("DELETE FROM departments WHERE id = @DepartmentId; DELETE FROM major_track WHERE Department_id = @DepartmentId;", conn);
     SqlParameter departmentIdParameter = new SqlParameter();
     departmentIdParameter.ParameterName = "@DepartmentId";
     departmentIdParameter.Value = this.GetId();

     cmd.Parameters.Add(departmentIdParameter);
     cmd.ExecuteNonQuery();

     if (conn != null)
     {
       conn.Close();
     }
   }

        public static void DeleteAll()
        {
            SqlConnection conn = DB.Connection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM departments;", conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public int GetId()
        {
            return _id;
        }

        public string GetCollege()
        {
            return _college;
        }
    }
}
