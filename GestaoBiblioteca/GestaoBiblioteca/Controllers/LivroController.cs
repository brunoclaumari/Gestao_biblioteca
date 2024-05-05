using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestaoBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivroController : ControllerBase
    {
        private readonly IRepository _repo;

        public LivroController(IRepository repo)
        {
            _repo = repo;
        }

        // GET: api/<LivroController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var retorno = await _repo.GetAllLivrosAsync();
            //var retorno = await teste;

            return Ok(retorno);
        }

        // GET api/<LivroController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LivroController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<LivroController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LivroController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
