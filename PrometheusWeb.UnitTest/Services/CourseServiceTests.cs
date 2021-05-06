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
    }
}