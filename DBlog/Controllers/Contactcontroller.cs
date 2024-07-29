using Microsoft.AspNetCore.Mvc;
using DBlog.Models;

namespace DBlog.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.Message = "Your message has been sent!";
                return View();
            }

            return View(model);
        }
    }
}
