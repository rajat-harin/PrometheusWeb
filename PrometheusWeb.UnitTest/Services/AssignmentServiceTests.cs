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
    public class AssignmentServiceTests
    {
        [TestMethod()]
        public void GetAssignmentsTest()
        {
            //Arrange
            AssignmentService assignmentService = new AssignmentService();
 
            //Act
            var result = assignmentService.GetAssignments();

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void GetAssignmentTest()
        {
            //Arrange
            AssignmentService assignmentService = new AssignmentService();
            int id = 1;
            //Act
            var result = assignmentService.GetAssignment(id);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AddAssignmentTest()
        {
            //Arrange
            AssignmentService assignmentService = new AssignmentService();
            AssignmentUserModel userModel = new AssignmentUserModel
            {
                AssignmentID = 99,
                CourseID = 1,
                HomeWorkID = 1,
                TeacherID = 1
            };

            //Act
            var result = assignmentService.AddAssignment(userModel);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void UpdateAssignmentTest()
        {
            //Arrange
            AssignmentService assignmentService = new AssignmentService();
            AssignmentUserModel userModel = new AssignmentUserModel
            {
                AssignmentID = 99,
                CourseID = 1,
                HomeWorkID = 1,
                TeacherID = 1
            };

            //Act
            var result = assignmentService.UpdateAssignment(userModel.AssignmentID,userModel);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void DeleteAssignmentTest()
        {
            //Arrange
            AssignmentService assignmentService = new AssignmentService();
            int id = 5;

            //Act
            var result = assignmentService.DeleteAssignment(id);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void AssignmentExistsTest()
        {
            //Arrange
            AssignmentService assignmentService = new AssignmentService();
            int id = 2;

            //Act
            var result = assignmentService.IsAssignmentExists(id);

            //Assert
            Assert.IsTrue(result);
        }
    }
}