using System;
using System.Threading.Tasks;

namespace Common.Web.Core
{
    /// <summary>
    /// Downloader Service
    /// </summary>
    public interface IDownloaderService
    {
        /// <summary>
        /// Downloads a file from a given url and then stores it as a local file.
        /// </summary>
        Task<DownloadStatus?> DownloadFileAsync(string url, string outputFilePath, AutoRetriesPolicy? autoRetries = null);

        /// <summary>
        /// Downloads a file from a given url and then returns it as a byte array.
        /// </summary>
        Task<(byte[] Data, DownloadStatus DownloadStatus)> DownloadDataAsync(string url, AutoRetriesPolicy? autoRetries = null);

        /// <summary>
        /// Downloads a file from a given url and then returns it as a text.
        /// </summary>
        Task<(string Data, DownloadStatus DownloadStatus)> DownloadPageAsync(string url, AutoRetriesPolicy? autoRetries = null);

        /// <summary>
        /// Gives the current download operation's status.
        /// </summary>
        Action<DownloadStatus>? OnDownloadStatusChanged { set; get; }

        /// <summary>
        /// It fires when the download operation is completed.
        /// </summary>
        Action<DownloadStatus>? OnDownloadCompleted { set; get; }

        /// <summary>
        /// Cancels the download operation.
        /// </summary>
        void CancelDownload();
    }
}