using AutoMapper;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Mappers;
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

        private readonly IMapper _mapper;
        /*

        public CourseService(CourseContext courseContext)
        {
            _courseContext = courseContext;
            _mapper = new CourseMapper().RetornaMapperConfiguration().CreateMapper();
        }
         */

        public LivroController(IRepository repo)
        {
            _repo = repo;
            _mapper = new BibliotecaMapper().RetornaMapperConfiguration().CreateMapper();
        }

        // GET: api/<LivroController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var retorno = await _repo.GetAllLivrosAsync();
            //var retorno = await teste;

            return Ok(retorno);
        }

        // GET api/<LivroController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {

            //Aluno aluno = _smartContext.Alunos.FirstOrDefault(x => x.Id == id);
            Livro livro = await _repo.GetLivroByIdAsync(id: id);

            if (livro is null) return BadRequest($"Aluno id = {id} não encontrado!!");
            //var alunoDTO = _mapper.Map<AlunoRegistrarDTO>(aluno);

            return Ok(livro);
        }

        // POST api/<LivroController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LivroDTO dto)
        {
            Livro livro = _mapper.Map<Livro>(dto);

            _repo.Add(livro);
            if (await _repo.SaveChangesAsync())
                return Created($"/api/Livro", _mapper.Map<LivroDTO>(livro));
            else
                return BadRequest("Falha ao salvar Livro.");
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
