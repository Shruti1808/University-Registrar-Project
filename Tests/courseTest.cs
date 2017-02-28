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

        public void Test_AddStudent_AddStudentTOCourse()
        {
            Course testCourse = new Course("Psychobiology", 1, "PSC121");
            testCourse.Save();

            Student testStudent1 = new Student("John","8-10-2009", 3);
            testStudent1.Save();

            Student testStudent2 = new Student("Joe","8-10-2006", 2);
            testStudent2.Save();

            testCourse.AddStudent(testStudent1);
            testCourse.AddStudent(testStudent2);

            List<Student> testList = new List<Student>{testStudent1,testStudent2};
            List<Student> result = testCourse.GetStudents();

            Assert.Equal(result,testList);
        }

        [Fact]
        public void Test_GetStudents_ReturnTheListOfStudents()
        {
            Course testCourse = new Course("Psychobiology", 1, "PSC121");
            testCourse.Save();

            Student testStudent1 =  new Student("Joe", "23-06-2011",2);
            testStudent1.Save();

            Student testStudent2 = new Student("Joe", "23-06-2011",2);
            testStudent2.Save();

            testCourse.AddStudent(testStudent1);
            List<Student> savedStudents = testCourse.GetStudents();
            List<Student> resultList = new List<Student> {testStudent1};

            Assert.Equal(savedStudents, resultList);
        }

        public void Dispose()
        {
            Course.DeleteAll();
            Student.DeleteAll();

        }

    }

}
