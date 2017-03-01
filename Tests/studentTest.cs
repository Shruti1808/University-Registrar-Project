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

        [Fact]
        public void Test_Find_ReturnsSameObject()
        {
            Student newStudent = new Student("John","8-10-2009", 3);
            newStudent.Save();

            Student testStudent = Student.Find(newStudent.GetId());
            Assert.Equal(testStudent,newStudent);
        }

        [Fact]
       public void Test_Update_UpdateStudent()
       {
           //Arrange
           string name = "Johnny";
           Student testInput = new Student("Johnny","8-10-2009", 3);
           testInput.Save();
           string newName = ("Joey");

           //Act
           testInput.Update(newName);
           string result = testInput.GetName();

           //Assert
           Assert.Equal(result, newName);
       }


        [Fact]
        public void Test_Delete_DeleteSingleStudent()
        {
          //Arrange
          Student testInput = new Student("John","8-10-2009", 3);
          testInput.Save();
          Student testInput2 = new Student ("Jose","8-10-2009", 3);
          testInput2.Save();

          //Act
          testInput.Delete();
          List<Student> result = Student.GetAll();
          List<Student> resultList = new List<Student> {testInput2};

          Assert.Equal(result, resultList);
        }


        [Fact]
        public void Test_AddCourse_AddCourseToStudent()
        {
            Student testStudent = new Student("John","8-10-2009", 3);
            testStudent.Save();

            Course testCourse =  new Course("Psychobiology", 1, "PSC121");
            testCourse.Save();

            Course testCourse2 = new Course("Psychobiology", 1, "PSC121");
            testCourse2.Save();

            testStudent.AddCourse(testCourse);
            testStudent.AddCourse(testCourse2);

            List<Course> testList = new List<Course> {testCourse, testCourse2};
            List<Course> result = testStudent.GetCourses();

            Assert.Equal(result, testList);
        }

        [Fact]
        public void Test_GetCourses_RetrieveListOfAllCourses()
        {
            Student testStudent = new Student("John","8-10-2009", 3);
            testStudent.Save();

            Course testCourse =  new Course("Psychobiology", 1, "PSC121");
            testCourse.Save();

            Course testCourse2 = new Course("Psychobiology", 1, "PSC121");
            testCourse2.Save();

            testStudent.AddCourse(testCourse);
            List<Course> savedCourses = testStudent.GetCourses();
            List<Course> resultList = new List<Course> {testCourse};

            Assert.Equal(savedCourses, resultList);
        }


        public void Dispose()
        {
            Student.DeleteAll();
            Course.DeleteAll();
        }

    }

}
