using System.Collections.Generic;
using InvertedxAPI.Collections;

namespace InvertedxAPI.Models
{
    public interface IRepository
    {
        IEnumerable<Website> WebsiteCollection { get; }
        InvertedIndex<Website> Index { get; }
        Website this[string id] { get; }

        Website AddWebsiteSource(Website website);
        Website UpdateWebsiteSource(Website website);
        void DeleteWebsiteSource(Website website);
    }
}