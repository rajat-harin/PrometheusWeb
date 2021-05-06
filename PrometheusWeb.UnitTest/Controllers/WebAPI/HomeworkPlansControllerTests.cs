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
    public class HomeworkPlansControllerTests
    {
        public readonly IHomeworkPlanService MockProductsRepository;
        public HomeworkPlansControllerTests()
        {

            // create some mock Items to play with
            IQueryable<HomeworkPlanUserModel> enrollments = new List<HomeworkPlanUserModel>
                {
                    new HomeworkPlanUserModel
                    {
                        HomeworkPlanID = 1,
                        isCompleted = false,
                        StudentID = 1,
                        HomeworkID = 1,
                        PriorityLevel = 1
                    },
                    new HomeworkPlanUserModel
                    {
                        HomeworkPlanID = 2,
                        isCompleted = false,
                        StudentID = 1,
                        HomeworkID = 2,
                        PriorityLevel = 2
                    }
                }.AsQueryable();
            // Mock the HomeworkPlan Repository using Moq
            Mock<IHomeworkPlanService> mockProductRepository = new Mock<IHomeworkPlanService>();

            // Return all the items
            mockProductRepository.Setup(mr => mr.GetHomeworkPlans()).Returns(enrollments);

            // return a item by Id
            mockProductRepository.Setup(mr => mr.GetHomeworkPlan(
                It.IsAny<int>())).Returns((int i) => enrollments.Where(
                x => x.HomeworkPlanID == i).Single());

            // Add a item   
            mockProductRepository.Setup(mr => mr.AddHomeworkPlan(
                It.IsAny<HomeworkPlanUserModel>())).Returns((HomeworkPlanUserModel target) => {
                    target.HomeworkPlanID = enrollments.Count() + 1;
                    enrollments.ToList().Add(target);
                    return true;
                });
            // Update a item   
            mockProductRepository.Setup(mr => mr.UpdateHomeworkPlan(It.IsAny<int>(), It.IsAny<HomeworkPlanUserModel>())).Returns((int id, HomeworkPlanUserModel target) => {
                var original = enrollments.Where(
                        q => q.HomeworkPlanID == target.HomeworkPlanID).Single();

                if (original == null)
                {
                    return false;
                }

                original.HomeworkPlanID = target.HomeworkPlanID;
                original.HomeworkID = target.HomeworkID;
                original.StudentID = target.StudentID;
                original.isCompleted = target.isCompleted;
                original.PriorityLevel = target.PriorityLevel;

                return true;
            });

            // Delete a item   
            mockProductRepository.Setup(mr => mr.DeleteHomeworkPlan(
                It.IsAny<int>())).Returns((int id) => {
                    var original = enrollments.Where(
                            q => q.HomeworkPlanID == id).Single();
                    enrollments.ToList().Remove(original);
                    return original;
                });

            // Complete the setup of our Mock HomeworkPlan Repository
            this.MockProductsRepository = mockProductRepository.Object;
        }

        [TestMethod()]
        public void GetHomeworkPlansTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHomeworkPlansTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetHomeworkPlanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PutHomeworkPlanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostHomeworkPlanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostHomeworkPlansTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteHomeworkPlanTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteHomeworkPlansTest()
        {
            Assert.Fail();
        }
    }
}