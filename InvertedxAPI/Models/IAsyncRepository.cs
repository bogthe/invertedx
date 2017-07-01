using System.Collections.Generic;
using System.Threading.Tasks;
using InvertedxAPI.Collections;

namespace InvertedxAPI.Models
{
    public interface IAsyncRepository
    {
        InvertedIndex<Website> Index { get; }
        Task Initialisation { get; }
        Task<List<Website>> Collection { get; }
        Task<Website> this[string id] { get; }

        Task<Website> AddWebsite(Website website);
        Task<Website> UpdateWebsite(Website website);
        Task DeleteWebsite(Website website);
    }
}