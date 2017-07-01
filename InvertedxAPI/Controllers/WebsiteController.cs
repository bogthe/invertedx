using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InvertedxAPI.Models;
using System.Threading.Tasks;

namespace InvertedxAPI.Controllers
{
    [Route("api/[controller]")]
    public class WebsiteController : Controller
    {
        private IAsyncRepository repository { get; set; }

        public WebsiteController(IAsyncRepository repo)
        {
            repository = repo;
            repository.Initialisation.Wait();
        }

        [HttpGet]
        public async Task<List<Website>> Get() => await repository.Collection;

        [HttpGet("{id}")]
        public async Task<Website> Get(string id) => await repository[id];

        [HttpPost]
        public async Task<Website> Post([FromBody]Website website) => await repository.AddWebsite(website);

        [HttpPut]
        public async Task<Website> Put([FromBody]Website website) => await repository.UpdateWebsite(website);

        [HttpDelete("{id}")]
        public async Task Delete(Website website) => await repository.DeleteWebsite(website);
    }
}