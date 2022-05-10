﻿using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Common.Web.Core
{
    /// <summary>
    /// DNTScheduler needs a ping service to keep it alive.
    /// This class provides the SiteRootUrl for the PingTask.
    /// </summary>
    public class MySitePingClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MySitePingClient> _logger;

        /// <summary>
        /// Pings the site's root url.
        /// </summary>
        public MySitePingClient(HttpClient httpClient, ILogger<MySitePingClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Pings the site's root url.
        /// </summary>
        public async Task WakeUp()
        {
            try
            {
                if (_httpClient.BaseAddress != null)
                {
                    await _httpClient.GetStringAsync(_httpClient.BaseAddress);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(0, ex.Demystify(), "Failed running the Ping task.");
            }
        }
    }
}