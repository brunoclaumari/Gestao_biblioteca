using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Repositories;
using GestaoBiblioteca.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestaoBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmprestimoController : ControllerBase
    {

        private readonly IRepository _repo;
        private readonly EmprestimoService _service;

        public EmprestimoController(IRepository repo, EmprestimoService service)
        {
            _repo = repo;
            _service = service;
        }


        // GET: api/<EmprestimoController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var retorno = await _repo.GetAllEmprestimosAsync();

                return Ok(retorno);

            }
            catch (Exception ex)
            {
                var response = new CustomResponse { IsSucessfull = false };
                response.StatusCode = 400;
                response.Message = "Ocorreu um erro na requisição!";
                return BadRequest(response);
            }
        }

        // GET: api/usuario/<EmprestimoController>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetAllUsuarioId(int usuarioId)
        {
            try
            {
                var retorno = await _repo.GetEmprestimosByUsuarioIdAsync(usuarioId);

                return Ok(retorno);

            }
            catch (Exception ex)
            {
                var response = new CustomResponse { IsSucessfull = false };
                response.StatusCode = 400;
                response.Message = "Ocorreu um erro na requisição!";
                return BadRequest(response);
            }
        }


        // GET api/<EmprestimoController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<EmprestimoController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmprestimoDTOEntrada entrada)
        {
            var emprestimo = _service.ConverteDTOParaEmprestimo(entrada);
            bool deuCerto = false;
            string mensagemErro = _service.VerificaEmprestimoValido(emprestimo);
            if (!string.IsNullOrEmpty(mensagemErro))
            {
                var response = new CustomResponse { IsSucessfull = false };
                response.StatusCode = 422;
                response.Message = mensagemErro;
                return UnprocessableEntity(response);
            }

            try
            {
                _repo.IniciaTransacaoAsync();
                _repo.Add(emprestimo);
                deuCerto = await _repo.SaveChangesAsync();
                if (deuCerto)
                {
                    _repo.ConfirmaTransacaoAsync();
                    return Created($"/api/{emprestimo.Id}", emprestimo);
                }
                else
                    return BadRequest("Falha ao salvar Aluno.");

            }
            catch (Exception ex)
            {
                _repo.CancelaTransacaoAsync();
                return BadRequest("Falha ao salvar Aluno.");
            }
            /*
             _repo.Add(aluno);
            if (_repo.SaveChanges())
                return Created($"/api/aluno/{aluno.Id}", _mapper.Map<AlunoDTO>(aluno));
            else
                return BadRequest("Falha ao salvar Aluno.");
             */
        }

        // PUT api/<EmprestimoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmprestimoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
