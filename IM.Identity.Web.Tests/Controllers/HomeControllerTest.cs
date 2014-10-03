using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IM.Identity.Web.Controllers;

namespace IM.Identity.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Users()
        {
            var controller = new UsersController();
            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Roles()
        {
            var controller = new RolesController();
            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }
    }
}
