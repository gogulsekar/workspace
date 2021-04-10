﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Services
{
    public interface ISortService
    {
        List<SortJob> Jobs { get; }
        void Enqueue(SortJob job);
        SortJob? GetJob(Guid jobId);
        List<SortJob> GetJobs();
        Task<SortJob> Process(SortJob job);
        void UpdateJob(SortJob job);
    }
}