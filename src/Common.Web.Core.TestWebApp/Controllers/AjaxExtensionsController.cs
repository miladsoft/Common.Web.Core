using Microsoft.AspNetCore.Mvc;

namespace Common.Web.Core.TestWebApp.Controllers
{
    public class AjaxExtensionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AjaxOnly]
        public IActionResult AjaxOnlyRequest()
        {
            return Json(new { IsAjaxRequest = this.HttpContext.Request.IsAjaxRequest() });
        }
    }
}