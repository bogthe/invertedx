using System.Collections.Generic;

namespace InvertedxAPI.Models
{
    public interface IRepository
    {
        IEnumerable<Website> WebsiteCollection { get; }
        Website this[int id] { get; }

        Website AddWebsiteSource(Website website);
        Website UpdateWebsiteSource(Website website);
        void DeleteWebsiteSource(int id);
    }
}