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
        private Timer _timer;
        static object lockObj = new object();

        public WorkerService(ISortService sortService, ILogger<WorkerService> logger)
        {
            _sortService = sortService;
            _logger = logger;
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
            List<SortJob> processed = new List<SortJob>();
            lock (lockObj)
            {
                var jobs = _sortService.Jobs.Where(p => p.Status == SortJobStatus.Pending);
                foreach (var job in jobs)
                {
                    var resultJob = _sortService.Process(job).Result;
                    processed.Add(resultJob);
                }

                foreach (var processedJob in processed)
                {
                    _logger.LogInformation($"Hosted Service is processing the job - {processedJob.Id}.");
                    _sortService.UpdateJob(processedJob);
                }
            }
        }
    }
}
