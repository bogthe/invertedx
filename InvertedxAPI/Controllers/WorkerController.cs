namespace InvertedxAPI.Controllers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [Route("api/[controller]")]
    public class WorkerController : Controller
    {
        private IRepository repository { get; set; }
        private IHttpHandler httpHandler;

        public WorkerController(IRepository repo)
        {
            repository = repo;
            httpHandler = new HttpHandler();
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            using (var worker = new ProcessorWorker(httpHandler))
            {
                Website website = repository[id];
                if (website == null)
                    return string.Empty;
                
                string websiteContent = worker.GetWebsiteContent(website, ContentProcessor);
                worker.PopulateIndex(repository.Index, websiteContent, website);
                
                return "Processed";
            }
        }

        private string ContentProcessor(string stringContent)
        {
            string pattern = @"\<p\>(.*?)\<\/p\>";
            MatchCollection matches = Regex.Matches(stringContent, pattern);
            List<string> content = new List<string>(matches.Count);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                content.Add(Regex.Replace(match.Groups[1].Value,"<[^>]*>",""));
            }

            return string.Join(" ", content);
        }
    }
}