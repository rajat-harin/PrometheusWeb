using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrometheusWeb.MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace PrometheusWeb.MVC.Controllers.Tests
{
    [TestClass()]
    public class StudentControllerTests
    {
        [TestMethod()]
        public void IndexTest()
        {
            //Arrange
            var controller = new StudentController();

            //Act
            var result = controller.Index().Result as ViewResult;
            //Assert
            Assert.IsNotNull(result); //Testing for Null result
            Assert.IsInstanceOfType(result, typeof(ViewResult)); // Type Checking
            Assert.AreEqual("Student Index Page", result.ViewBag.Title); // Testing if VIewbag title is same
            

        }

        [TestMethod()]
        public void MyCoursesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ViewCoursesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHomeworksTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void EnrollInCourseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHomeworkPlanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GeneratePlanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdatePlanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdatePlanTest1()
        {
            Assert.Fail();
        }
    }
}