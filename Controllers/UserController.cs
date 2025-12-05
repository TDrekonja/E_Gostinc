using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_Gostinc.Models;
using System.Text.Encodings.Web;

namespace E_Gostinc.Controllers;

public class UserController : Controller
{
    public IActionResult User(string name)
    {
        ViewData["UserName"] = name;
        return View();
    }
}