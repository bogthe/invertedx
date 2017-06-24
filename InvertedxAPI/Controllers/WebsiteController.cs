using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using InvertedxAPI.Models;

namespace InvertedxAPI.Controllers
{
    [Route("api/[controller]")]
    public class WebsiteController : Controller
    {
        private IRepository repository { get; set; }

        public WebsiteController(IRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public IEnumerable<Website> Get() => repository.WebsiteCollection;

        [HttpGet("{id}")]
        public Website Get(int id) => repository[id];

        [HttpPost]
        public Website Post([FromBody]Website website) => repository.AddWebsiteSource(website);

        [HttpPut]
        public Website Put([FromBody]Website website) => repository.UpdateWebsiteSource(website);

        [HttpDelete("{id}")]
        public void Delete(int id) => repository.DeleteWebsiteSource(id);
    }
}