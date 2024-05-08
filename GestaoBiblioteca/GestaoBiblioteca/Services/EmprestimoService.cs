using GestaoBiblioteca.Context;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Helpers;
using GestaoBiblioteca.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace GestaoBiblioteca.Services
{
    public class EmprestimoService
    {

        private readonly GestaoBibliotecaContext _context;

        private readonly IRepository _repo;

        public Emprestimo EmprestimoAntesDeAlterar { get; set; } = new Emprestimo();

        //public ICollection<ItensEmprestimo> ItensEmprestimoAntesAlterar { get; set; }

        public EmprestimoService(GestaoBibliotecaContext context, IRepository repo)
        {
            //context = new GestaoBibliotecaContext();
            _context = context;
            _repo = repo;
        }

        public Emprestimo Clone(Emprestimo e)
        {
            return new Emprestimo
            {
                Id = e.Id,
                DataDevolucao = e.DataDevolucao,
                DataEmprestimo = e.DataEmprestimo,
                ItensEmprestimos = e.ItensEmprestimos,
                StatusEmprestimo = e.StatusEmprestimo,
                Usuario = e.Usuario,
                UsuarioId = e.UsuarioId
            };
        }

        /// <summary>
        /// Faz verificações do usuário para ver se ele é válido
        /// </summary>
        /// <param name="emprestimo"></param>
        /// <returns></returns>
        public List<string> VerificaEmprestimoValido(Emprestimo emprestimo)
        {
            /*
             Um empréstimo de livro só pode ser permitido se:
              * O usuário estiver cadastrado no banco
              * Se o usuário não possui pendências
              * O livro estiver cadastrado no banco
              * Possuir livros disponíveis
             */
            var listaErros = new List<string>();
            string mensagemErro = string.Empty;

            int usuarioId = emprestimo.UsuarioId;
            var usuario = _context.Usuarios.FirstOrDefault(x=>x.Id == usuarioId);
            if (usuario == null)
            {
                mensagemErro = $"Não existe usuário com o id: {usuarioId} fornecido";
                listaErros.Add(mensagemErro);
            }
            else
            {
               var emprestimosPendentes =  _context.Emprestimos.ToList().FindAll(x => x.DataDevolucao < DateTime.Now);
                if(emprestimosPendentes != null && emprestimosPendentes.Count > 0)
                {
                    mensagemErro = $"Usuário id: {usuarioId} possui empréstimos pendentes. Não é permitido novos empréstimos";
                    listaErros.Add(mensagemErro);
                }
            }
            FazVerificacoesLivros(emprestimo, listaErros);

            return listaErros;

        }

        private void FazVerificacoesLivros(Emprestimo emprestimo, List<string> listaErros)
        {            
            var listaLivros = _context.Livros.ToList();

            emprestimo.ItensEmprestimos.ToList().ForEach(x =>
            {
                var livro = listaLivros.FirstOrDefault(lv => lv.Id == x.LivroId);

                //if (!listaLivros.Contains(x.Livro))
                if (livro == null)
                {
                    string msg = $"O livro id: {x.LivroId} não está cadastrado.";
                    listaErros.Add(msg);
                }
                else
                {
                    if(livro.RetornaQuantidadeDisponivel() == 0)
                    {
                        string msg = $"Os exemplares do {x.Livro.ToString()} estão todos emprestados.";
                        listaErros.Add(msg);
                    }
                }
            });                        
        }

        public Emprestimo ConverteDTOParaEmprestimo(EmprestimoPadraoDTO dto, bool fazRegistroNovo)
        {
            Emprestimo emprestimo = new Emprestimo();
            List<ItensEmprestimo> itensEmprestimos = new List<ItensEmprestimo>();
            
            if (fazRegistroNovo)
            { 
                EmprestimoDTOEntrada dtoEntrada = (EmprestimoDTOEntrada)dto;
                dtoEntrada.ItensEmprestimos.ToList().ForEach(i =>
                {
                    ItensEmprestimo item = new ItensEmprestimo
                    {
                        LivroId = i.LivroId,
                        //EmprestimoId = dto.Id,
                    };
                    itensEmprestimos.Add(item);
                });
                emprestimo = new Emprestimo()
                {                    
                    ItensEmprestimos = itensEmprestimos,
                    //StatusEmprestimo = dto.StatusEmprestimo,
                    UsuarioId = dtoEntrada.UsuarioId,
                };
            }
            else
            {
                emprestimo = EmprestimoAntesDeAlterar;
                emprestimo.StatusEmprestimo = ((EmprestimoDTOUpdate)dto).StatusEmprestimo;
            } 

            return emprestimo;
        }

        

        
        public async Task<CustomResponse> FazSalvar(EmprestimoPadraoDTO entrada, bool fazRegistroNovo)
        {
            var emprestimo = this.ConverteDTOParaEmprestimo(entrada, fazRegistroNovo);
            AdicionaDatasEmprestimo(emprestimo, fazRegistroNovo);
            var mensagensErro = this.VerificaEmprestimoValido(emprestimo);
            try
            {
                if (mensagensErro.Count > 0)
                {
                    return ObtemRetornoComErro(mensagensErro, (int)HttpStatusCode.UnprocessableEntity);
                }
                _repo.IniciaTransacaoAsync();
                //ApagaItensEmprestimoSubstituidos();
                _repo.SalvaOuAtualiza(emprestimo, fazRegistroNovo);
                AtualizaQuantidadeLivrosEmprestados(emprestimo, fazRegistroNovo);
                bool deuCerto = await _repo.SaveChangesAsync();
                if (deuCerto)
                {
                    _repo.ConfirmaTransacaoAsync();
                    var status = fazRegistroNovo? HttpStatusCode.Created : HttpStatusCode.OK;
                    return _repo.ObtemResponseSucesso(emprestimo, status);                    
                }
                else
                {
                    mensagensErro.Add("Falha ao salvar Empréstimo.");
                    return ObtemRetornoComErro(mensagensErro, (int)HttpStatusCode.BadRequest);                    
                }

            }
            catch (Exception ex)
            {
                _repo.CancelaTransacaoAsync();
                mensagensErro = new List<string>();
                mensagensErro.Add($"Falha inesperada ao salvar Empréstimo - Exception: {ex.Message}");
                return ObtemRetornoComErro(mensagensErro, (int)HttpStatusCode.BadRequest);
                //var response = new CustomResponse { FoiSucesso = false, StatusCode = 400 };
                //response.ListaMensagens.Add($"Falha inesperada ao salvar Empréstimo - Exception: {ex.Message}");
                //return BadRequest(response);
            }

        }

        public void AtualizaQuantidadeLivrosEmprestados(Emprestimo emprestimo, bool fazRegistroNovo)
        {
            if (fazRegistroNovo)
            {
                emprestimo.ItensEmprestimos.ToList().ForEach(i =>
                {
                    _repo.EmprestaLivro(i.LivroId);
                });
            }
            else
            {   
                if(emprestimo.StatusEmprestimo == Enums.EnumEmprestimoStatus.Devolvido)
                {
                    foreach (var itensEmprestimo in emprestimo.ItensEmprestimos.ToList())
                    {
                        //_repo.DevolveLivro(itensEmprestimo.LivroId);
                        if(itensEmprestimo.Livro != null)
                        {
                            itensEmprestimo.Livro.DevolveLivro();
                            Console.WriteLine($"Devolver: {itensEmprestimo.LivroId}");
                        }
                    }
                }
                //Verifica se houve troca de livros no update
                //List<int> itensAnteriores = ItensEmprestimoAntesAlterar.Select(x => x.LivroId).OrderBy(x => x).ToList();
                //List<int> itensAtuais = emprestimo.ItensEmprestimos.Select(x => x.LivroId).ToList().OrderBy(x => x).ToList();

                //DevolveLivrosEmprestaLivros(itensAnteriores, itensAtuais);

            }
        }

        //private void DevolveLivrosEmprestaLivros(List<int> anterior, List<int> atual)
        //{
        //    List<int> devolver = anterior.Except(atual).ToList();
        //    List<int> emprestar = atual.Except(anterior).ToList();

        //    //Console.WriteLine(devolver.Count);
        //    foreach (int livroId in devolver)
        //    {
        //        _repo.DevolveLivro(livroId);
        //        Console.WriteLine($"Devolver: {livroId}");
        //    }

        //    //Console.WriteLine(emprestar.Count);
        //    foreach (int livroId in emprestar)
        //    {
        //        _repo.EmprestaLivro(livroId);
        //        Console.WriteLine($"Emprestar: {livroId}");
        //    }
        //}

        private void AdicionaDatasEmprestimo(Emprestimo emprestimo, bool fazRegistroNovo)
        {
            if(fazRegistroNovo)
            {
                DateTime dataAtual = DateTime.Today; // Data atual com hora definida como meia-noite
                DateTime dataComHora = dataAtual.AddHours(23).AddMinutes(59);
                emprestimo.DataEmprestimo = dataComHora;
                emprestimo.DataDevolucao = dataComHora.AddDays(7);
            }
        }

        public CustomResponse ObtemRetornoComErro(List<string> listaErros, int statusCode)
        {
            var response = new CustomResponse { FoiSucesso = false };
            response.StatusCode = statusCode;
            response.ListaMensagens = listaErros;

            return response;
        }

        
    }
}
