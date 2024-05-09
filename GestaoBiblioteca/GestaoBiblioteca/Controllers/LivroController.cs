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
    public class LivroController : ControllerBase
    {
        private readonly IRepository _repo;
        private readonly IMapper _mapper;
        private readonly BibliotecaService _service;
        private const string _erroInesperado = "Ocorreu um erro inesperado na requisição!";
        private const string _mensagemDeFalha = "Falha ao {0} o livro. Verifique os dados";
        private const string _livroIdNaoEncontrado = "Livro id = {0} não encontrado!!";

        

        public LivroController(IRepository repo, BibliotecaService service)
        {
            _repo = repo;
            _mapper = new BibliotecaMapper().RetornaMapperConfiguration().CreateMapper();
            _service = service;
        }

        /// <summary>
        /// Retorna todos os livros cadastrados
        /// </summary>
        /// <returns></returns>
        // GET: api/<LivroController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var retorno = await _repo.GetAllLivrosAsync();            

            return Ok(retorno);
        }

        /// <summary>
        /// Retorna o livro pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET api/<LivroController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var listaErros = new List<string>();
            try
            {
                var livro = await _repo.GetLivroByIdAsync(id: id);
                if (livro is null)
                {
                    listaErros.Add(string.Format(_livroIdNaoEncontrado, id));
                    return UnprocessableEntity(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }

                return Ok(livro);
                //return Ok(_repo.ObtemResponseSucesso(livro, HttpStatusCode.OK));

            }
            catch (Exception ex)
            {
                listaErros.Add(_erroInesperado);
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }

            
            //_repo.ObtemResponseSucesso(emprestimo, status); 
        }

        /// <summary>
        /// Cadastra um livro novo
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        // POST api/<LivroController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LivroDTOEntrada dto)
        {
            var listaErros = new List<string>();
            string acao = "salvar";
            try
            {                
                Livro livro = _mapper.Map<Livro>(dto);

                _repo.Add(livro);
                bool deuCerto = await _repo.SaveChangesAsync();
                if (deuCerto)
                {                    
                    return Created($"/api/Livro", _mapper.Map<LivroDTORetorno>(livro));
                }
                else
                {                    
                    listaErros.Add(string.Format(_mensagemDeFalha, acao));
                    return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));                    
                }

            }
            catch (Exception ex)
            {                
                listaErros.Add(_erroInesperado);
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }
        }

        /// <summary>
        /// Atualiza informações do livro
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entrada"></param>
        /// <returns></returns>
        // PUT api/<LivroController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LivroDTOEntrada entrada)
        {
            var listaErros = new List<string>();
            string acao = "atualizar";
            try
            {
                Livro? livro = await _repo.GetLivroByIdAsync(id: id);

                if (livro is null)
                {
                    listaErros.Add(string.Format(_livroIdNaoEncontrado, id));
                    return UnprocessableEntity(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }
  
                var livroUpdate =_mapper.Map(entrada, livro);

                _repo.Update(livroUpdate);

                if (await _repo.SaveChangesAsync())
                    return Ok(_mapper.Map<LivroDTORetorno>(livroUpdate));
                else
                {
                    listaErros.Add(string.Format(_mensagemDeFalha, acao));
                    return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
                }
            }
            catch (Exception ex)
            {
                listaErros.Add(_erroInesperado);
                return BadRequest(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.BadRequest));
            }

            
        }

        /// <summary>
        /// Exclui um livro pelo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE api/<LivroController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var listaErros = new List<string>();
            string acao = "deletar";
            try
            {
                Livro? livro = await _repo.GetLivroByIdAsync(id: id);

                if (livro is null)
                {
                    listaErros.Add(string.Format(_livroIdNaoEncontrado, id));
                    return UnprocessableEntity(_service.ObtemRetornoComErro(listaErros, (int)HttpStatusCode.UnprocessableEntity));
                }
                _repo.Delete(livro);
                if (await _repo.SaveChangesAsync())
                {
                    return Ok(new CustomResponse { FoiSucesso = true,  ListaMensagens = new List<string> { $"O livro id: {id} foi deletado com sucesso!" }});
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
