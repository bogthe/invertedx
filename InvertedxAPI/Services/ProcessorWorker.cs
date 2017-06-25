using System;
using System.Collections.Generic;
using System.Net.Http;
using InvertedxAPI.Collections;
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

        public void Dispose()
        {
            Dispose(true);
        }

        public string GetWebsiteContent(Website website, Func<string, string> extractor)
        {
            if(website == null || extractor == null)
                return string.Empty;
            
            string content = httpClient.GetUrlContentAsync(website.Url).Result;
            website.Processed = true;

            return extractor.Invoke(content);
        }
        
        public void PopulateIndex(InvertedIndex<Website> index, string content, Website web)
        {
            foreach(var word in content.Split(' '))
            {
                string key = ProcessKey(word);
                if(!index.ContainsKey(key))
                    index.Add(key, new HashSet<Website>());
                
                index[key].Add(web);
            }
        }

        private string ProcessKey(string key) => key.ToLower();

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
    }
}