using System.Collections.Generic;

namespace InvertedxAPI.Models
{
    public class MemoryRepository : IRepository
    {
        private Dictionary<int, Website> items;

        public MemoryRepository()
        {
            items = new Dictionary<int, Website>();
            new List<Website>
            {
                new Website(){Url = "https://en.wikipedia.org/wiki/Bogdan"},
                new Website(){Url = "https://en.wikipedia.org/wiki/Inverted_index"}
            }.ForEach(w => AddWebsiteSource(w));
        }

        public Website this[int id] => items.ContainsKey(id) ? items[id] : null;

        public IEnumerable<Website> WebsiteCollection => items.Values;

        public Website AddWebsiteSource(Website website)
        {
            if (website.Id == 0)
            {
                int id = items.Count == 0 ? 1 : items.Count;
                while (items.ContainsKey(id)) { id++; }
                website.Id = id;
            }
            items[website.Id] = website;
            return website;
        }

        public Website UpdateWebsiteSource(Website website)
            => AddWebsiteSource(website);

        public void DeleteWebsiteSource(int id) => items.Remove(id);
    }
}