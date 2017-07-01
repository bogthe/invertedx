using System;
using System.Collections.Generic;
using InvertedxAPI.Collections;

namespace InvertedxAPI.Models
{
    public class MemoryRepository : IRepository
    {
        private Dictionary<string, Website> items;
        private InvertedIndex<Website> index;

        public MemoryRepository()
        {
            items = new Dictionary<string, Website>();
            index = new InvertedIndex<Website>();

            new List<Website>
            {
                new Website(){Url = "https://en.wikipedia.org/wiki/Bogdan"},
                new Website(){Url = "https://en.wikipedia.org/wiki/Inverted_index"}
            }.ForEach(w => AddWebsiteSource(w));
        }

        public Website this[string id] => items.ContainsKey(id) ? items[id] : null;

        public IEnumerable<Website> WebsiteCollection => items.Values;

        public InvertedIndex<Website> Index => index;

        public Website AddWebsiteSource(Website website)
        {
            if (string.IsNullOrEmpty(website.Id))
            {
                website.Id = Guid.NewGuid().ToString();
            }

            items[website.Id] = website;
            return website;
        }

        public Website UpdateWebsiteSource(Website website)
            => AddWebsiteSource(website);

        public void DeleteWebsiteSource(Website website) => items.Remove(website.Id);
    }
}