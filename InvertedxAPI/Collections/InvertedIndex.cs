using System.Collections.Generic;

namespace InvertedxAPI.Collections
{
    public class InvertedIndex<T> : IInvertedIndex<T>
    {
        private SortedDictionary<string, HashSet<T>> items = new SortedDictionary<string, HashSet<T>>();

        public HashSet<T> this[string id]
        {
            get => items[id];
        }
        
        public void Add(string key, HashSet<T> newValue) => items.Add(key, newValue);
        
        public bool ContainsKey(string key) => items.ContainsKey(key);
        public IEnumerator<string> GetKeys() => items.Keys.GetEnumerator();
    }
}