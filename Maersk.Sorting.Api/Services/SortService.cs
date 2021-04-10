using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Services
{
    public class SortService : ISortService
    {
        private static List<SortJob> _jobs = new List<SortJob>();
        private readonly ISortJobProcessor _processor;

        public List<SortJob> Jobs
        {
            get
            {
                return _jobs;
            }
        }

        
        public SortService(ISortJobProcessor processor)
        {
            _processor = processor;
        }

        public void Enqueue(SortJob job)
        {
            _jobs.Add(job);
        }

        public SortJob? GetJob(Guid jobId)
        {
            var job = _jobs.Find(p => p.Id.Equals(jobId));
            return job;
        }

        public List<SortJob> GetJobs()
        {
            return _jobs;
        }

        public async Task<SortJob> Process(SortJob job)
        {
            var status = await _processor.Process(job);
            return status;
        }

        public void UpdateJob(SortJob job)
        {
            var jobsNeedToBeUpdated = _jobs.Find(p => p.Id.Equals(job.Id));
            if (jobsNeedToBeUpdated == null)
            {
                return;
            }

            var jobToBeUpdated = _jobs.Remove(jobsNeedToBeUpdated);
            _jobs.Add(job);
        }
    }
}
