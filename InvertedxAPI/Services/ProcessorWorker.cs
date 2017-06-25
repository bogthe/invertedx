using System;
using System.Net.Http;
using InvertedxAPI.Models;

namespace InvertedxAPI.Services
{
    public class ProcessorWorker : IDisposable
    {
        private IHttpHandler httpClient;
        private bool disposedValue = false;

        public ProcessorWorker(IHttpHandler client)
        {
            httpClient = client;
        }

        public string GetWebsiteContent(Website website, Func<string, string> extractor)
        {
            if(website == null || extractor == null)
                return string.Empty;
            
            string content = httpClient.GetUrlContentAsync(website.Url).Result;
            website.Processed = true;

            return extractor.Invoke(content);
        }

        private void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    httpClient.Dispose();
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