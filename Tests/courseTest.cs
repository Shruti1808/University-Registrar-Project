using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using University;

namespace University
{
    public class CourseTest : IDisposable
    {
        public CourseTest()
        {
            DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=university_test;Integrated Security=SSPI;";
        }

        [Fact]
        public void TEST_CheckDBIsEmpty()
        {
            int result = Course.GetAll().Count;

            Assert.Equal(0, result);
        }

        [Fact]
        public void Test_Equal_ReturnsTrueIfDetailsAreTheSame()
        {
            //Arrange, Act
            Course firstCourse = new Course("Psychobiology", 1, "PSC121");
            Course secondCourse = new Course("Psychobiology", 1, "PSC121");

            //Assert
            Assert.Equal(firstCourse, secondCourse);
        }

        [Fact]
        public void Test_Save_AssignsIdToObject()
        {
          //Arrange

          Course testCourse = new Course("Psychobiology", 1, "PSC121");

          //Act
          testCourse.Save();
          Course savedCourse = Course.GetAll()[0];

          int result = savedCourse.GetId();
          int testId = testCourse.GetId();

          //Assert
          Assert.Equal(testId, result);
        }

        [Fact]
        public void Test_FindReturnsSameObject()
        {
            Course newCourse = new Course("Psychobiology", 1, "PSC121");
            newCourse.Save();

            Course testCourse = Course.Find(newCourse.GetId());

            Assert.Equal(testCourse,newCourse);
        }

        public void Dispose()
        {
            Course.DeleteAll();

        }

    }

}
