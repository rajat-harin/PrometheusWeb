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
    public class TeachesControllerTests
    {
        public readonly ITeachesService MockProductsRepository;
        public TeachesControllerTests()
        {

            // create some mock Items to play with
            IQueryable<TeacherCourseUserModel> enrollments = new List<TeacherCourseUserModel>
                {
                    new TeacherCourseUserModel
                    {
                        TeacherCourseID = 1,
                        TeacherID = 1,
                        CourseID = 1
                    },
                    new TeacherCourseUserModel
                    {
                        TeacherCourseID = 2,
                        TeacherID = 1,
                        CourseID = 2
                    }
                }.AsQueryable();
            // Mock the Teaches Repository using Moq
            Mock<ITeachesService> mockProductRepository = new Mock<ITeachesService>();

            // Return all the items
            mockProductRepository.Setup(mr => mr.GetTeacherCourses()).Returns(enrollments);

            // return a item by Id
            mockProductRepository.Setup(mr => mr.GetTeacherCourses(
                It.IsAny<int>())).Returns((int i) => enrollments.Where(
                x => x.TeacherCourseID == i).Single());

            // Add a item   
            mockProductRepository.Setup(mr => mr.AddTeacherCourse(
                It.IsAny<TeacherCourseUserModel>())).Returns((TeacherCourseUserModel target) => {
                    target.TeacherCourseID = enrollments.Count() + 1;
                    enrollments.ToList().Add(target);
                    return true;
                });
            // Update a item   
            mockProductRepository.Setup(mr => mr.UpdateTeacherCourses(It.IsAny<int>(), It.IsAny<TeacherCourseUserModel>())).Returns((int id, TeacherCourseUserModel target) => {
                var original = enrollments.Where(
                        q => q.TeacherCourseID == target.TeacherCourseID).Single();

                if (original == null)
                {
                    return false;
                }

                original.TeacherCourseID = target.TeacherCourseID;
                original.CourseID = target.CourseID;
                original.TeacherID = target.TeacherID;

                return true;
            });

            // Delete a item   
            mockProductRepository.Setup(mr => mr.DeleteTeacherCourse(
                It.IsAny<int>())).Returns((int id) => {
                    var original = enrollments.Where(
                            q => q.TeacherCourseID == id).Single();
                    enrollments.ToList().Remove(original);
                    return original;
                });

            // Complete the setup of our Mock TeacherCourse Repository
            this.MockProductsRepository = mockProductRepository.Object;
        }

        [TestMethod()]
        public void GetTeacherCoursesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetTeacherCoursesTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutTeacherCoursesTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostTeacherCourseTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteTeacherCourseTest()
        {
            Assert.Fail();
        }
    }
}