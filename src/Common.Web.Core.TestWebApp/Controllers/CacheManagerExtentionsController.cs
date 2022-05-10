using Microsoft.AspNetCore.Mvc;

namespace Common.Web.Core.TestWebApp.Controllers
{
    public class CacheManagerExtentionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AjaxOnly]
        [NoBrowserCache]
        public IActionResult NoBrowserCacheRequest()
        {
            return Json(new { IsAjaxRequest = this.HttpContext.Request.IsAjaxRequest() });
        }
    }
}