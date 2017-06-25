using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvertedxAPI.Services
{
    public class HttpHandler : IHttpHandler
    {
        private HttpClient client = new HttpClient();
        private bool disposedValue = false;
        
        public async Task<string> GetUrlContentAsync(string url)
        {
            return await client.GetStringAsync(url);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}