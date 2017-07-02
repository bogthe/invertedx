using System.Collections.Generic;

namespace InvertedxAPI.Collections
{
    public interface IInvertedIndex<T>
    {
        HashSet<T> this[string id] { get; }
        bool ContainsKey(string key);
        IEnumerator<string> GetKeys();
    }
}