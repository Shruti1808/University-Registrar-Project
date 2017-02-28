using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using University;

namespace University
{
    public class StudentTest : IDisposable
    {
        public StudentTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void TEST_CheckDBIsEmpty()
        {
            int result = Student.GetAll().Count;

            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueIfDetailsAreTheSame()
        {
            //Arrange, Act
            Student firstStudent = new Student("Joe", "2000-01-01", 1);
            Student secondStudent = new Student("Joe", "2000-01-01", 1);

            //Assert
            Assert.Equal(firstStudent, secondStudent);
        }

        [Fact]
        public void Test_Save_AssignsIdToObject()
        {
          //Arrange

          Student testStudent = new Student("Joe", "2-12-2012", 2);

          //Act
          testStudent.Save();
          Student savedStudent = Student.GetAll()[0];

          int result = savedStudent.GetId();
          int testId = testStudent.GetId();

          //Assert
          Assert.Equal(testId, result);
        }

        public void Dispose()
        {
            Student.DeleteAll();

        }



    }

}
