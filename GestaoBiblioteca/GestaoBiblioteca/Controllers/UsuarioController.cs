using AutoMapper;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Mappers;
using GestaoBiblioteca.Repositories;
using GestaoBiblioteca.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestaoBiblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;
        private readonly BibliotecaService _service;
        private const string _erroInesperado = "Ocorreu um erro inesperado na requisição!";
        private const string _mensagemDeFalha = "Falha ao {0} o usuário. Verifique os dados";
        private const string _usuarioNaoEncontrado = "Usuário id = {0} não encontrado!!";

        public UsuarioController(IRepository repo, BibliotecaService service)
        {
            _repo = repo;
            _mapper = new BibliotecaMapper().RetornaMapperConfiguration().CreateMapper();
            _service = service;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados
        /// </summary>
        /// <returns></returns>
        // GET: api/<UsuarioController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var retorno = await _repo.GetAllUsuariosAsync(false);

            return Ok(retorno);
        }

        /// <summary>
        /// Retorna um usuário pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var listaErros = new List<string>();
            try
            {
                Usuario? usuario = await _repo.GetUsuarioByIdAsync(id: id);
                if (usuario is null)
                {
                    listaErros.Add(string.Format(_usuarioNaoEncontrado, id));
                    return UnprocessableEntity(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }

                return Ok(usuario);                

            }
            catch (Exception ex)
            {
                listaErros.Add(_erroInesperado);
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }            
        }

        /// <summary>
        /// Cadastra um novo usuário
        /// </summary>
        /// <param name="entrada"></param>
        /// <returns></returns>
        // POST api/<UsuarioController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UsuarioDTO entrada)
        {
            var listaErros = new List<string>();
            string acao = "salvar";
            try
            {
                listaErros = await _service.RetornaErrosValidacaoUsuarioNoRegistroAsync(entrada);
                if (listaErros.Count == 0)
                {
                    Usuario usuario = _mapper.Map<Usuario>(entrada);
                    usuario.DataRegistro = DateTime.Now;
                     _repo.Add(usuario);
                    bool deuCerto = await _repo.SaveChangesAsync();
                    if (deuCerto)
                    {
                        return Created($"/api/Usuario", _mapper.Map<UsuarioDTORetorno>(usuario));
                    }
                }

                listaErros.Add(string.Format(_mensagemDeFalha, acao));
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                listaErros.Add(_erroInesperado);
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }
        }

        /// <summary>
        /// Atualiza dados de um usuário
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entrada"></param>
        /// <returns></returns>
        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UsuarioDTO entrada)
        {
            var listaErros = new List<string>();
            string acao = "atualizar";
            try
            {
                Usuario? usuario = await _repo.GetUsuarioByIdAsync(id: id);

                if (usuario is null)
                {
                    listaErros.Add(string.Format(_usuarioNaoEncontrado, id));
                    return UnprocessableEntity(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }

                listaErros = await _service.RetornaErrosValidacaoUsuarioNoRegistroAsync(entrada, false);
                
                if(listaErros.Count == 0)
                {                    
                    _mapper.Map(entrada, usuario);
                    usuario.DataAtualizacao = DateTime.Now;
                    _repo.Update(usuario);

                    if (await _repo.SaveChangesAsync())
                        return Ok(_mapper.Map<UsuarioDTORetorno>(usuario));
                }

                listaErros.Add(string.Format(_mensagemDeFalha, acao));
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                listaErros.Add(_erroInesperado);
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }


        }

        /// <summary>
        /// Apaga um usuário cadastrado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {            
            var listaErros = new List<string>();
            string acao = "deletar";
            try
            {
                Usuario? usuario = await _repo.GetUsuarioByIdAsync(id: id);

                if (usuario is null)
                {
                    listaErros.Add(string.Format(_usuarioNaoEncontrado, id));
                    return UnprocessableEntity(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }

                listaErros = _service.RetornaErrosValidacaoUsuarioNoDelete(usuario);
                if (listaErros.Count == 0)
                {
                    _repo.Delete(usuario);
                    if (await _repo.SaveChangesAsync())
                    {
                        return Ok(new CustomResponse { FoiSucesso = true, ListaMensagens = new List<string> { $"O livro id: {id} foi deletado com sucesso!" } });
                    }
                }

                listaErros.Add(string.Format(_mensagemDeFalha, acao));
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }
            catch (Exception ex)
            {
                listaErros.Add(_erroInesperado);
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }
        }
    }
}
