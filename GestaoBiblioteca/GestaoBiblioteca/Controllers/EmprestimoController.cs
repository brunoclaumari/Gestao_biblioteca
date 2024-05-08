using Azure;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Repositories;
using GestaoBiblioteca.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
                var response = new CustomResponse { FoiSucesso = false };
                response.StatusCode = 400;
                response.ListaMensagens.Add("Ocorreu um erro na requisição!");
                return BadRequest(response);
            }
        }

        // GET: api/usuario/<EmprestimoController>
        /// <summary>
        /// Obtém o Empréstimo de livros pelo id do Usuário
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <returns></returns>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetAllUsuarioId(int usuarioId)
        {
            try
            {
                var listaRetorno = await _repo.GetEmprestimosByUsuarioIdAsync(usuarioId);

                if (listaRetorno is null || listaRetorno.Count == 0)
                {
                    var listaErros = new List<string>();
                    listaErros.Add($"O usuário id = {usuarioId} não possui empréstimos ou usuário não existe!!");
                    return UnprocessableEntity(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }

                return Ok(listaRetorno);

            }
            catch (Exception ex)
            {
                var response = new CustomResponse { FoiSucesso = false };
                response.StatusCode = 400;
                response.ListaMensagens.Add("Ocorreu um erro inesperado na requisição!");
                return BadRequest(response);
            }
        }


        // GET api/<EmprestimoController>/5
        /// <summary>
        /// Obtém o Empréstimo de livros por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                Emprestimo emprestimo = await _repo.GetEmprestimoByIdAsync(id);

                if (emprestimo is null)
                {
                    var listaErros = new List<string>();
                    listaErros.Add($"Empréstimo id = {id} não encontrado!!");
                    return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }

                return Ok(emprestimo);
            }
            catch (Exception e)
            {
                var erro = new CustomResponse { FoiSucesso = false };
                erro.ListaMensagens.Add("Ocorreu um erro inesperado. Verifique os dados de entrada");
                return BadRequest(erro);
            }
        }

        // POST api/<EmprestimoController>
        /// <summary>
        /// Adiciona um novo empréstimo
        /// </summary>
        /// <param name="entrada"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmprestimoDTOEntrada entrada)
        {
            try
            {
                Task<CustomResponse> response = _service.FazSalvar(entrada, true);
                return await ObtemResponses(response);
            }
            catch (Exception e)
            {
                var erro = new CustomResponse { FoiSucesso = false };
                erro.ListaMensagens.Add("Ocorreu um erro inesperado. Verifique os dados de entrada");
                return BadRequest(erro);
            }            
        }
        

        // PUT api/<EmprestimoController>/5
        /// <summary>
        /// Atualiza Empréstimo passando o id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entrada"></param>
        /// <returns></returns>
        [HttpPut("{id}")]        
        public async Task<IActionResult> Put(int id, [FromBody] EmprestimoDTOUpdate entrada)
        {
            var erro = new CustomResponse { FoiSucesso = false };
            erro.ListaMensagens.Add("Ocorreu um erro inesperado. Verifique os dados de entrada");
            try
            {
                Emprestimo emprestimo = await _repo.GetEmprestimoByIdAsync(id);

                if (emprestimo is null)
                {
                    var listaErros = new List<string>();
                    listaErros.Add($"Empréstimo id = {id} não encontrado!!");
                    return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }

                _repo.IniciaTransacaoAsync();

                emprestimo.StatusEmprestimo = entrada.StatusEmprestimo;
                _service.AtualizaQuantidadeLivrosEmprestados(emprestimo, false);

                _repo.Update(emprestimo);
                bool deuCerto = await _repo.SaveChangesAsync();
                if (deuCerto)
                {
                    _repo.ConfirmaTransacaoAsync();
                    return Ok(emprestimo);
                }
                else
                {
                    _repo.CancelaTransacaoAsync();
                    return BadRequest(erro);
                }
                //return (IActionResult)Task.FromResult(_repo.ObtemResponseSucesso(emprestimo, HttpStatusCode.OK));
                //_service.EmprestimoAntesDeAlterar = emprestimo;
                //Task<CustomResponse> response = _service.FazSalvar(entrada, false);
                //return await ObtemResponses(response);

            }
            catch (Exception e)
            {
                _repo.CancelaTransacaoAsync();
                
                return BadRequest(erro);
            }


        }



        // DELETE api/<EmprestimoController>/5
        /// <summary>
        /// Apaga um Empréstimo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = new CustomResponse { FoiSucesso = false };
            try
            {
                Emprestimo emprestimo = await _repo.GetEmprestimoByIdAsync(id);
                if (emprestimo is null)
                {
                    response.StatusCode = 422;
                    response.ListaMensagens.Add($"Empréstimo id = {id} não encontrado!!");
                    return UnprocessableEntity(response);
                }
                _repo.Delete(emprestimo);
                if (await _repo.SaveChangesAsync())
                    return NoContent();
                else
                {
                    response.StatusCode = 400;
                    response.ListaMensagens.Add($"Não foi possível excluir o Empréstimo id: {id}");
                    return BadRequest(response);
                }

            }
            catch (Exception e)
            {                
                response.ListaMensagens.Add("Ocorreu um erro inesperado. Verifique os dados de entrada");
                return BadRequest(response);
            }
        }

        private async Task<IActionResult> ObtemResponses(Task<CustomResponse> response)
        {
            switch ((HttpStatusCode)response.Result.StatusCode)
            {
                case HttpStatusCode.UnprocessableEntity:
                    return UnprocessableEntity(await response);
                case HttpStatusCode.BadRequest:
                    return BadRequest(await response);
                default:
                    //int id = response.Entidade.Id;
                    return Created(uri: $"/api", await response);
                    //break;
            }
        }
    }
}
