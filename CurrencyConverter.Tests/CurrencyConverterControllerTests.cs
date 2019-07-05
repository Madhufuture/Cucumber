namespace CurrencyConverter.Tests
{
    using Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Moq;
    using Repository.Interfaces;
    using ViewModels;
    using Xunit;

    public class CurrencyConverterControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;

        public CurrencyConverterControllerTests()
        {
            _mockLogger=new Mock<ILogger<HomeController>>();
        }

        private UserDetailsVm InvalidName()
        {
            return new UserDetailsVm
            {
                Name = "",
                Amount = (float) 140.32
            };
        }

        private UserDetailsVm InvalidAmount()
        {
            return new UserDetailsVm
            {
                Name = "",
                Amount = 140_32
            };
        }

        private UserDetailsVm ValidModelData()
        {
            return new UserDetailsVm
            {
                Name = "Madhu",
                Amount = 120
            };
        }

        private UserData SampleUserData()
        {
            return new UserData
            {
                Name = "Madhu",
                Amount = "OneHundredTwenty"
            };
        }

        [Fact]
        public void HomeController_GetInstance_Parameterized_Test()
        {
            var mockRepo = new Mock<IConverter>();
            var homeController = new HomeController(mockRepo.Object,_mockLogger.Object);

            Assert.NotNull(homeController);
        }

        [Fact]
        public void HomeController_Index_ReturnsAViewResult()
        {
            var mockRepo = new Mock<IConverter>();
            var controller = new HomeController(mockRepo.Object,_mockLogger.Object);
            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void HomeController_Index_Post_InvalidName_ModelStateInvalid()
        {
            var mockRepo = new Mock<IConverter>();
            mockRepo.Setup(x => x.ConvertCurrency(It.IsAny<string>())).Returns("OneHundredTwenty");

            var controller = new HomeController(mockRepo.Object,_mockLogger.Object);
            controller.ModelState.AddModelError("Name", "Required");

            var result = controller.Index(InvalidName());
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void HomeController_Index_Post_InvalidAmount_ModelStateInvalid()
        {
            var mockRepo = new Mock<IConverter>();
            mockRepo.Setup(x => x.ConvertCurrency(It.IsAny<string>())).Returns("OneHundredTwenty");

            var controller = new HomeController(mockRepo.Object,_mockLogger.Object);
            controller.ModelState.AddModelError("Amount", "The field Amount must be a number.");

            var result = controller.Index(InvalidAmount());
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void HomeController_Index_Post_ModelStateValid()
        {
            var mockRepo = new Mock<IConverter>();
            mockRepo.Setup(x => x.ConvertCurrency(It.IsAny<string>())).Returns("OneHundredTwenty");

            var controller = new HomeController(mockRepo.Object,_mockLogger.Object);
            var result = controller.Index(ValidModelData());

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Details", redirectToActionResult.ActionName);
            mockRepo.Verify(x => x.ConvertCurrency(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void HomeController_Details_ReturnsAViewResult()
        {
            var mockRepo = new Mock<IConverter>();
            var controller = new HomeController(mockRepo.Object,_mockLogger.Object);

            var result = controller.Details(SampleUserData());
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UserDataVm>(viewResult.ViewData.Model);
            Assert.Equal("Madhu",model.Name);
            Assert.Equal("OneHundredTwenty",model.Currency);
        }
        
    }
}