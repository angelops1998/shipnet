using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace shipnet.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    public IActionResult Index() => View();
}