using System.Collections.Generic;
using InvertedxAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvertedxAPI.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private IRepository repository { get; set; }

        public SearchController(IRepository repo)
        {
            repository = repo;
        }

        [HttpGet("{id}")]
        public IEnumerable<Website> Get(string id) 
            => repository.Index.ContainsKey(id) ? repository.Index[id] : new HashSet<Website>();
    }
}