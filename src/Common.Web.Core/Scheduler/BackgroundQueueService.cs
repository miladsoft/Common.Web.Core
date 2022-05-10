using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.Web.Core
{
    /// <summary>
    /// BackgroundQueue Service
    /// A .NET Core replacement for the old HostingEnvironment.QueueBackgroundWorkItem.
    /// </summary>
    public class BackgroundQueueService : BackgroundService, IBackgroundQueueService
    {
        private readonly ILogger<BackgroundQueueService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IJobsRunnerTimer _jobsRunnerTimer;
        private readonly ConcurrentQueue<Func<CancellationToken, IServiceProvider, Task>> _asyncTasksQueue = new();
        private readonly ConcurrentQueue<Action<CancellationToken, IServiceProvider>> _syncTasksQueue = new();

        /// <summary>
        /// BackgroundQueue Service
        /// </summary>
        public BackgroundQueueService(
            ILogger<BackgroundQueueService> logger,
            IServiceScopeFactory serviceScopeFactory,
            IJobsRunnerTimer jobsRunnerTimer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            _jobsRunnerTimer = jobsRunnerTimer ?? throw new ArgumentNullException(nameof(jobsRunnerTimer));
        }

        /// <summary>
        /// This method is called when the IHostedService starts.
        /// </summary>
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Background Queue Service is starting.");

            if (_jobsRunnerTimer.IsRunning)
            {
                return Task.CompletedTask;
            }

            _jobsRunnerTimer.OnThreadPoolTimerCallback = () =>
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    return;
                }

                var tasks = new List<Task>();

                while (!_asyncTasksQueue.IsEmpty)
                {
                    if (_asyncTasksQueue.TryDequeue(out var asyncWorkItem))
                    {
                        tasks.Add(runTaskAsync(asyncWorkItem, stoppingToken));
                    }
                }

                while (!_syncTasksQueue.IsEmpty)
                {
                    if (_syncTasksQueue.TryDequeue(out var workItem))
                    {
                        tasks.Add(Task.Run(() => runTaskSync(workItem, stoppingToken)));
                    }
                }

                if (tasks.Any())
                {
                    Task.WaitAll(tasks.ToArray());
                }
            };
            _jobsRunnerTimer.StartJobs();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _jobsRunnerTimer.StopJobs();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Queues a background Task
        /// </summary>
        public void QueueBackgroundWorkItem(Func<CancellationToken, IServiceProvider, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _asyncTasksQueue.Enqueue(workItem);
        }

        /// <summary>
        /// Queues a background Task
        /// </summary>
        public void QueueBackgroundWorkItem(Action<CancellationToken, IServiceProvider> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _syncTasksQueue.Enqueue(workItem);
        }

        private async Task runTaskAsync(Func<CancellationToken, IServiceProvider, Task> workItem, CancellationToken ct)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            try
            {
                await workItem(ct, serviceScope.ServiceProvider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Demystify(), $"Error occurred executing {nameof(workItem)}.");
            }
        }

        private void runTaskSync(Action<CancellationToken, IServiceProvider> workItem, CancellationToken ct)
        {
            using var serviceScope = _serviceScopeFactory.CreateScope();
            try
            {
                workItem(ct, serviceScope.ServiceProvider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Demystify(), $"Error occurred executing {nameof(workItem)}.");
            }
        }
    }
}