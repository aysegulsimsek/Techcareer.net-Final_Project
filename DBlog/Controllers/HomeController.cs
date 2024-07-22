using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DBlog.Models;

namespace DBlog.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

}
