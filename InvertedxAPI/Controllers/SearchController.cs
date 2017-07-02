using System.Collections.Generic;
using InvertedxAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvertedxAPI.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private IIndexRepository repository { get; set; }

        public SearchController(IIndexRepository repo)
        {
            repository = repo;
            repository.Initialisation.Wait();
        }

        [HttpGet("{id}")]
        public IEnumerable<string> Get(string id) 
            => repository.Index.ContainsKey(id) ? repository.Index[id] : new HashSet<string>();
    }
}