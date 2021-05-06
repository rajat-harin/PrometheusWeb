using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PrometheusWeb.Data.UserModels;
using PrometheusWeb.Services.Controllers;
using PrometheusWeb.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrometheusWeb.Services.Controllers.Tests
{
    [TestClass()]
    public class AssignmentsControllerTests
    {
        public readonly IAssignmentService MockProductsRepository;
        public AssignmentsControllerTests()
        {
            // create some mock Items to play with
            IQueryable<AssignmentUserModel> enrollments = new List<AssignmentUserModel>
                {
                    new AssignmentUserModel
                    {
                        AssignmentID = 1,
                        CourseID = 1,
                        HomeWorkID = 1,
                        TeacherID = 1
                    },
                    new AssignmentUserModel
                    {
                        AssignmentID = 2,
                        CourseID = 2,
                        HomeWorkID = 2,
                        TeacherID = 1
                    }
                }.AsQueryable();
            // Mock the Assignment Repository using Moq
            Mock<IAssignmentService> mockProductRepository = new Mock<IAssignmentService>();

            // Return all the items
            mockProductRepository.Setup(mr => mr.GetAssignments()).Returns(enrollments);

            // return a item by Id
            mockProductRepository.Setup(mr => mr.GetAssignment(
                It.IsAny<int>())).Returns((int i) => enrollments.Where(
                x => x.AssignmentID == i).Single());

            // Add a item   
            mockProductRepository.Setup(mr => mr.AddAssignment(
                It.IsAny<AssignmentUserModel>())).Returns((AssignmentUserModel target) => {
                    target.AssignmentID = enrollments.Count() + 1;
                    enrollments.ToList().Add(target);
                    return true;
                });
            // Update a item   
            mockProductRepository.Setup(mr => mr.UpdateAssignment(It.IsAny<int>(), It.IsAny<AssignmentUserModel>())).Returns((int id, AssignmentUserModel target) => {
                var original = enrollments.Where(
                        q => q.AssignmentID == target.AssignmentID).Single();

                if (original == null)
                {
                    return false;
                }

                original.AssignmentID = target.AssignmentID;
                original.CourseID = target.CourseID;
                original.TeacherID = target.TeacherID;
                original.CourseID = target.CourseID;

                return true;
            });

            // Delete a item   
            mockProductRepository.Setup(mr => mr.DeleteAssignment(
                It.IsAny<int>())).Returns((int id) => {
                    var original = enrollments.Where(
                            q => q.AssignmentID == id).Single();
                    enrollments.ToList().Remove(original);
                    return original;
                });

            // Complete the setup of our Mock Assignment Repository
            this.MockProductsRepository = mockProductRepository.Object;
        }

        [TestMethod()]
        public void GetAssignmentsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAssignmentTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutAssignmentTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostAssignmentTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteAssignmentTest()
        {
            Assert.Fail();
        }
    }
}