using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Services
{
    public interface ISortService
    {
        void Enqueue(SortJob job);
        SortJob? GetJob(Guid jobId);
        List<SortJob> GetJobs();
        Task<SortJob> Process(SortJob job);
        void UpdateJob(SortJob job);
    }
}
