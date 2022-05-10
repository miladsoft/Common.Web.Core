using Microsoft.AspNetCore.Mvc;

namespace Common.Web.Core.TestWebApp.Controllers
{
    public class DomainHelperExtensionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}