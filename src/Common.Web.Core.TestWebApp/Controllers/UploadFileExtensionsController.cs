using System.Threading.Tasks;
using Common.Web.Core.TestWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Common.Web.Core.TestWebApp.Controllers
{
    public class UploadFileExtensionsController : Controller
    {
        private readonly IUploadFileService _uploadFileService;

        public UploadFileExtensionsController(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadPhoto(UserFileViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var formFile = userViewModel.Photo;
                var (isSaved, savedFilePath) = await _uploadFileService.SavePostedFileAsync(formFile,allowOverwrite: false, "images", "nestedfolder", "nestedfolder2");
                if (!isSaved)
                {
                    ModelState.AddModelError("", "Uploaded file is null or empty.");
                    return View(viewName: "Index");
                }

                RedirectToAction("Index");
            }
            return View(viewName: "Index");
        }
    }
}