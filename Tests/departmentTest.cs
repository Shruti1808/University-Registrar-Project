using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using University;

namespace University
{
    public class DepartmentTest : IDisposable
    {
        public DepartmentTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void TEST_CheckDBIsEmpty()
        {
            int result = Department.GetAll().Count;

            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueIfDetailsAreTheSame()
        {
            //Arrange, Act
            Department firstDepartment = new Department("Humanities");
            Department secondDepartment = new Department("Humanities");

            //Assert
            Assert.Equal(firstDepartment, secondDepartment);
        }

        [Fact]
        public void Test_Save_AssignsIdToObject()
        {
            //Arrange

            Department testDepartment = new Department("Humanities");

            //Act
            testDepartment.Save();
            Department savedDepartment = Department.GetAll()[0];

            int result = savedDepartment.GetId();
            int testId = testDepartment.GetId();

            //Assert
            Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_FindReturnsSameObject()
        {
            Department newDepartment = new Department("Humanities");
            newDepartment.Save();

            Department testDepartment = Department.Find(newDepartment.GetId());

            Assert.Equal(testDepartment,newDepartment);
        }

        [Fact]
        public void Test_Delete_DeleteSingleDepartment()
        {
          //Arrange
          Department testInput = new Department("Humanities");
          testInput.Save();
          Department testInput2 = new Department ("Engineering");
          testInput2.Save();

          //Act
          testInput.Delete();
          List<Department> result = Department.GetAll();
          List<Department> resultList = new List<Department> {testInput2};

          Assert.Equal(result, resultList);
        }

        [Fact]
       public void Test_Update_UpdateStudent()
       {
           //Arrange
           string college = "Humanities";
           Department testInput = new Department(college);
           testInput.Save();
           string newCollege = ("Engineering");

           //Act
           testInput.Update(newCollege);
           string result = testInput.GetCollege();

           //Assert
           Assert.Equal(result, newCollege);
       }

        public void Dispose()
        {
            Department.DeleteAll();
            Student.DeleteAll();

        }

    }

}
