using Maersk.Sorting.Api.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Services
{
    public class WorkerService : IHostedService, IDisposable
    {
        private readonly ISortService _sortService;
        private readonly ILogger<WorkerService> _logger;
        private readonly InMemoryCache _cache;
        private Timer _timer;
        static object lockObj = new object();

        public WorkerService(ISortService sortService, ILogger<WorkerService> logger, InMemoryCache cache)
        {
            _sortService = sortService;
            _logger = logger;
            _cache = cache;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service running.");

            _timer = new Timer(callback: DoWork,
                               null,
                               TimeSpan.Zero,
                               TimeSpan.FromSeconds(10));

            return Task.CompletedTask;

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Hosted Service is stopping.");
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            foreach (var jobId in _cache.Keys)
            {
                var job = _cache.GetEntry(jobId);
                if (job != null && job.Status == SortJobStatus.Pending)
                {
                    var resultJob = _sortService.Process(job).Result;
                    _cache.SetEntry(jobId, resultJob);
                }
            }
        }
    }
}
