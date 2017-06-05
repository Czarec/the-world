using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IConfigurationRoot _config;
        private readonly IWorldRepository _repository;
        private readonly ILogger<AppController> _logger;

        public AppController(
            IMailService mailService,
            IConfigurationRoot config,
            IWorldRepository repository,
            ILogger<AppController> logger)
        {
            _mailService = mailService;
            _config = config;
            _repository = repository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var data = await _repository.GetAllTrips();
                return View(data);
            }            
            catch(Exception ex)
            {
                _logger.LogError($"Failed to get trips in Index page: {ex.Message}");
                return Redirect("/error");
            }
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From TheWorld", model.Message);
                ModelState.Clear();
                ViewBag.UserMessage = "Sent";
            }            
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
