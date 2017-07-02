namespace InvertedxAPI.Controllers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services;

    [Route("api/[controller]")]
    public class WorkerController : Controller
    {
        private IAsyncRepository repository { get; set; }
        private IIndexRepository indexRepository {get;set;}
        private IHttpHandler httpHandler;

        public WorkerController(IAsyncRepository repo, IIndexRepository indexRepo)
        {
            httpHandler = new HttpHandler();
            
            repository = repo;
            indexRepository = indexRepo;
            Task.WaitAll(repository.Initialisation, indexRepository.Initialisation);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            using (var worker = new ProcessorWorker(httpHandler))
            {
                Website website = await repository[id];
                if (website == null)
                    return NotFound();

                string websiteContent = await worker.GetWebsiteContent(website, ContentProcessor);
                worker.PopulateIndex(indexRepository.Index, websiteContent, website);
                await repository.UpdateWebsite(website);
                await indexRepository.UpdateRepo();

                return Ok();
            }
        }

        private string ContentProcessor(string stringContent)
        {
            string pattern = @"\<p\>(.*?)\<\/p\>";
            MatchCollection matches = Regex.Matches(stringContent, pattern);
            List<string> content = new List<string>(matches.Count);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                content.Add(Regex.Replace(match.Groups[1].Value, "<[^>]*>", ""));
            }

            return string.Join(" ", content);
        }
    }
}