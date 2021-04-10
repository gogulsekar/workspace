using Maersk.Sorting.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Controllers
{
    [ApiController]
    [Route("sort")]
    public class SortController : ControllerBase
    {
        private readonly ISortJobProcessor _sortJobProcessor;
        private readonly ISortService _service;

        public SortController(ISortJobProcessor sortJobProcessor, ISortService sortService)
        {
            _sortJobProcessor = sortJobProcessor;
            _service = sortService;
        }

        [HttpPost("run")]
        [Obsolete("This executes the sort job asynchronously. Use the asynchronous 'EnqueueJob' instead.")]
        public async Task<ActionResult<SortJob>> EnqueueAndRunJob(int[] values)
        {
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            var completedJob = await _sortJobProcessor.Process(pendingJob);

            return Ok(completedJob);
        }

        [HttpPost]
        public ActionResult<SortJob> EnqueueJob(int[] values)
        {
            // TODO: Should enqueue a job to be processed in the background.
            var pendingJob = new SortJob(
                id: Guid.NewGuid(),
                status: SortJobStatus.Pending,
                duration: null,
                input: values,
                output: null);

            _service.Enqueue(pendingJob);

            return Ok(pendingJob);
        }

        [HttpGet]
        public ActionResult<SortJob[]> GetJobs()
        {
            // TODO: Should return all jobs that have been enqueued (both pending and completed).
            var data = _service.GetJobs();
            return Ok(data);
        }

        [HttpGet("{jobId}")]
        public ActionResult<SortJob> GetJob(Guid jobId)
        {
            // TODO: Should return a specific job by ID.
            var data = _service.GetJob(jobId);
            return Ok(data);
        }
    }
}
