namespace CurrencyConverter.Controllers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Repository.Interfaces;
    using ViewModels;

    public class HomeController : Controller
    {
        private readonly IConverter _converter;
        private readonly ILogger _logger;
       
        public HomeController(IConverter converter,ILogger<HomeController> logger)
        {
            _converter = converter;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(UserDetailsVm model)
        {
            var userData = new UserData();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var valueFromRepo =
                _converter.ConvertCurrency(Convert.ToString(model.Amount, CultureInfo.InvariantCulture));
            userData.Name = model.Name;
            userData.Amount = valueFromRepo;

            return RedirectToAction("Details", userData);
        }

        public IActionResult Details(UserData userData)
        {
            var data = new UserDataVm
            {
                Name = userData.Name,
                Currency = userData.Amount
            };
            return View(data);
        }
    }
}