using GestaoBiblioteca.Context;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
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

        public EmprestimoService(GestaoBibliotecaContext context, IRepository repo)
        {
            //context = new GestaoBibliotecaContext();
            _context = context;
            _repo = repo;
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
                    string msg = $"O {x.Livro.ToString()} não está cadastrado.";
                    listaErros.Add(msg);
                }
                else
                {
                    if(livro.QuantidadeTotal == 0)
                    {
                        string msg = $"Os exemplares do {x.Livro.ToString()} estão todos emprestados.";
                        listaErros.Add(msg);
                    }
                }
            });                        
        }

        public Emprestimo ConverteDTOParaEmprestimo(EmprestimoDTOEntrada dto)
        {
            List<ItensEmprestimo> itensEmprestimos = new List<ItensEmprestimo>();
            dto.ItensEmprestimos.ToList().ForEach(i =>
            {
                ItensEmprestimo item = new ItensEmprestimo
                {
                    LivroId = i.LivroId,
                    //EmprestimoId = i.EmprestimoId,    
                };
                itensEmprestimos.Add(item);                
            });
            Emprestimo emprestimo = new Emprestimo()
            { 
                ItensEmprestimos = itensEmprestimos,
                StatusEmprestimo = dto.StatusEmprestimo,
                UsuarioId = dto.UsuarioId,                
            };

            if(dto is EmprestimoDTOUpdate)
            {
                emprestimo.DataEmprestimo = ((EmprestimoDTOUpdate)dto).DataEmprestimo;
                emprestimo.DataDevolucao = ((EmprestimoDTOUpdate)dto).DataDevolucao;
            }

            return emprestimo;
        }

        
        public async Task<CustomResponse> FazSalvar(EmprestimoDTOEntrada entrada, bool fazRegistroNovo)
        {
            var emprestimo = this.ConverteDTOParaEmprestimo(entrada);
            AdicionaDatasEmprestimo(emprestimo, fazRegistroNovo);
            var mensagensErro = this.VerificaEmprestimoValido(emprestimo);
            try
            {
                if (mensagensErro.Count > 0)
                {
                    return ObtemRetornoComErro(mensagensErro, (int)HttpStatusCode.UnprocessableEntity);
                }
                _repo.IniciaTransacaoAsync();
                _repo.SalvaOuAtualiza(emprestimo, fazRegistroNovo);
                AtualizaQuantidadeLivro(emprestimo, fazRegistroNovo);
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

        private void AtualizaQuantidadeLivro(Emprestimo emprestimo, bool fazRegistroNovo)
        {
            emprestimo.ItensEmprestimos.ToList().ForEach(i =>
            {
                i.Livro.QuantidadeTotal = i.Livro.QuantidadeTotal - 1;
            });
        }

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
