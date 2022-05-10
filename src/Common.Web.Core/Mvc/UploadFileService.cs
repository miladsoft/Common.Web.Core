using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Common.Web.Core
{
    /// <summary>
    /// Upload File Service
    /// </summary>
    public class UploadFileService : IUploadFileService
    {
        private const int MaxBufferSize = 0x10000; // 64K. The artificial constraint due to win32 api limitations. Increasing the buffer size beyond 64k will not help in any circumstance, as the underlying SMB protocol does not support buffer lengths beyond 64k.

        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Upload File Service
        /// </summary>
        public UploadFileService(IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// Saves the posted IFormFile to the specified directory asynchronously.
        /// </summary>
        /// <param name="formFile">The posted file.</param>
        /// <param name="allowOverwrite">Creates a unique file name if the file already exists.</param>
        /// <param name="destinationDirectoryNames">Directory names in the wwwroot directory.</param>
        /// <returns></returns>
        public async Task<(bool IsSaved, string SavedFilePath)> SavePostedFileAsync(
            IFormFile formFile, bool allowOverwrite, params string[] destinationDirectoryNames)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return (false, string.Empty);
            }

            var uploadsRootFolder = Path.Combine(_environment.WebRootPath);

            if (destinationDirectoryNames is not null)
            {
                for (int counter = 0; counter < destinationDirectoryNames.Length; counter++)
                {
                    uploadsRootFolder = Path.Combine(uploadsRootFolder, destinationDirectoryNames[counter]);
                }
            }

            if (!Directory.Exists(uploadsRootFolder))
            {
                Directory.CreateDirectory(uploadsRootFolder);
            }

            var filePath = Path.Combine(uploadsRootFolder, formFile.FileName);
            if (File.Exists(filePath) && !allowOverwrite)
            {
                filePath = getUniqueFilePath(formFile, uploadsRootFolder);
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write,
                                       FileShare.None,
                                       MaxBufferSize,
                                       // you have to explicitly open the FileStream as asynchronous
                                       // or else you're just doing synchronous operations on a background thread.
                                       useAsync: true))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return (true, filePath);
        }

        /// <summary>
        /// Saves the posted IFormFile to a byte array.
        /// </summary>
        /// <param name="formFile">The posted file.</param>
        public async Task<byte[]?> GetPostedFileDataAsync(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static string getUniqueFilePath(IFormFile formFile, string uploadsRootFolder)
        {
            var fileName = Path.GetFileNameWithoutExtension(formFile.FileName);
            var extension = Path.GetExtension(formFile.FileName);
            return Path.Combine(uploadsRootFolder, $"{fileName}.{DateTime.Now.ToString("yyyyMMddHHmmssfff", CultureInfo.InvariantCulture)}.{Guid.NewGuid().ToString("N")}{extension}");
        }
    }
}