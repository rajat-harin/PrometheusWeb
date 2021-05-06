using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrometheusWeb.Data.UserModels;
using PrometheusWeb.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrometheusWeb.Services.Services.Tests
{
    [TestClass()]
    public class CourseServiceTests
    {
        [TestMethod()]
        public void AddCourseTest()
        {
            //Arrange
            CourseService courseService = new CourseService();
            CourseUserModel userModel = new CourseUserModel
            {
                Name = "Test",
                CourseID = 2120,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            //Act
            var result = courseService.AddCourse(userModel);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void DeleteCourseTest()
        {
            //Arrange
            CourseService courseService = new CourseService();
            int id = 5;

            //Act
            var result = courseService.DeleteCourse(id);

            //Assert
            Assert.IsNotNull(result);
        }

        public void GetCourseTest()
        {
            //Arrange
            CourseService courseService = new CourseService();
            int id = 2;

            //Act
            var result = courseService.GetCourse(id);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void GetCoursesTest()
        {
            //Arrange
            CourseService courseService = new CourseService();

            //Act
            var result = courseService.GetCourses();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void UpdateCourseTest()
        {
            //Arrange
            CourseService courseService = new CourseService();
            CourseUserModel userModel = new CourseUserModel
            {
                Name = "Test",
                CourseID = 2120,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };

            //Act
            var result = courseService.UpdateCourse(5, userModel);

            //Assert
            Assert.IsTrue(result);
        }
        

        [TestMethod()]
        public void CourseExistsTest()
        {
            //Arrange
            CourseService enrollmentService = new CourseService();
            int id = 2;

            //Act
            var result = enrollmentService.IsCourseExists(id);

            //Assert
            Assert.IsTrue(result);
        }
    }
}