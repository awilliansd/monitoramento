using Microsoft.AspNetCore.Mvc;

namespace Monitoramento.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}