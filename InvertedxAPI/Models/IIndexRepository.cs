using System.Threading.Tasks;
using InvertedxAPI.Collections;

namespace InvertedxAPI.Models
{
    public interface IIndexRepository
    {
        Task Initialisation { get;}
        InvertedIndex<string> Index { get; }
        Task UpdateRepo();
    }
}