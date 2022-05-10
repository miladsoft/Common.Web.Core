using Microsoft.AspNetCore.Mvc;

namespace Common.Web.Core.TestWebApp.Controllers
{
    public class ControllerExtensionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RedirectToCommonHttpClientFactory()
        {
            return RedirectToAction(nameof(Index),
                                    typeof(CommonHttpClientFactoryController).ControllerName());
        }
    }
}