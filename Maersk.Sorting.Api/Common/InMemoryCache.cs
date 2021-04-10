using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maersk.Sorting.Api.Common
{
    public class InMemoryCache
    {
        private Dictionary<string, SortJob> _cache = new Dictionary<string, SortJob>();

        public InMemoryCache()
        {
        }

        public List<string> Keys
        {
            get
            {
                return _cache.Keys.ToList<string>();
            }
        }

        public SortJob? GetEntry(string jobId)
        {
            if (_cache.ContainsKey(jobId))
            {
                return _cache[jobId];
            }
            return null;
        }

        public void SetEntry(string jobId, SortJob job)
        {
            if (_cache.ContainsKey(jobId))
            {
                _cache[jobId] = job;
            }
            else
            {
                _cache.Add(jobId, job);
            }
        }

        public List<SortJob> GetEntries()
        {
           return _cache.Values.ToList<SortJob>();
        }
    }
}
