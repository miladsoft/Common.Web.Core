using System.Threading.Tasks;
using Common.Web.Core.TestWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Common.Web.Core.TestWebApp.Controllers
{
    public class AllowUploadSafeFilesController : Controller
    {
        private readonly IUploadFileService _uploadFileService;

        public AllowUploadSafeFilesController(IUploadFileService uploadFileService)
        {
            _uploadFileService = uploadFileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadGeneralFile(GeneralFileViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var formFile = userViewModel.UserFile;
                var (isSaved, savedFilePath) = await _uploadFileService.SavePostedFileAsync(formFile, allowOverwrite: false, "files", "nestedfolder", "nestedfolder2");
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