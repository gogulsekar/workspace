using Maersk.Sorting.Api.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Services
{
    public class SortService : ISortService
    {
        private readonly ISortJobProcessor _processor;
        private readonly InMemoryCache _cache;

        public SortService(ISortJobProcessor processor, InMemoryCache cache)
        {
            _processor = processor;
            _cache = cache;
        }

        public void Enqueue(SortJob job)
        {

            _cache.SetEntry(job.Id.ToString(), job);
        }

        public SortJob? GetJob(Guid jobId)
        {
            var job = _cache.GetEntry(jobId.ToString());
            return job;
        }

        public List<SortJob> GetJobs()
        {
            return _cache.GetEntries();
        }

        public async Task<SortJob> Process(SortJob job)
        {
            var status = await _processor.Process(job);
            return status;
        }

        public void UpdateJob(SortJob job)
        {
            _cache.SetEntry(job.Id.ToString(), job);
        }
    }
}
