using System;
using System.Threading.Tasks;

namespace InvertedxAPI.Services
{
    public interface IHttpHandler : IDisposable
    {
        Task<string> GetUrlContentAsync(string url);
    }
}