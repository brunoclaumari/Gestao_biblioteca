using GestaoBiblioteca.Context;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using GestaoBiblioteca.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GestaoBiblioteca.Services
{
    public class BibliotecaService
    {

        private readonly GestaoBibliotecaContext _context;

        private readonly IRepository _repo;

        public Emprestimo EmprestimoAntesDeAlterar { get; set; } = new Emprestimo();

        //public ICollection<ItensEmprestimo> ItensEmprestimoAntesAlterar { get; set; }

        public BibliotecaService(GestaoBibliotecaContext context, IRepository repo)
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
                
               var emprestimosVencidos =  _context.Emprestimos.ToList().FindAll(x => x.DataDevolucao < DateTime.Now);
                //if(emprestimosVencidos != null && emprestimosVencidos.Count > 0)
                if (UsuarioPossuiEmprestimoValidoEmAberto(usuarioId))
                {
                    mensagemErro = $"Usuário id: {usuarioId} possui empréstimo vigente. Não é permitido novos empréstimos";
                    listaErros.Add(mensagemErro);
                }
                if (UsuarioPossuiEmprestimoNaoDevolvidoEmAberto(usuarioId))
                {
                    mensagemErro = $"Usuário id: {usuarioId} possui empréstimo não devolvido! " +
                        $"Entre em contato pelo número {usuario.Telefone}";
                    listaErros.Add(mensagemErro);
                }
            }
            FazVerificacoesLivros(emprestimo, listaErros);

            return listaErros;

        }

        public bool UsuarioPossuiEmprestimoValidoEmAberto(int usuarioId)
        {
            DateTime diaHoje = DateTime.Now.Date;
            bool possuiEmprestimoAberto = false;
            var emp = _repo.GetEmprestimosByUsuarioIdAsync(usuarioId).Result;

            possuiEmprestimoAberto = emp != null && 
                (emp.Any(x => x.DataDevolucao.Date.Subtract(diaHoje).Days >= 0 && x.StatusEmprestimo == Enums.EnumEmprestimoStatus.EmAberto));

            return possuiEmprestimoAberto;
        }

        public bool UsuarioPossuiEmprestimoNaoDevolvidoEmAberto(int usuarioId)
        {
            DateTime diaHoje = DateTime.Now.Date;
            bool possuiEmprestimoNaoDevolvido = false;
            var emp = _repo.GetEmprestimosByUsuarioIdAsync(usuarioId).Result;

            possuiEmprestimoNaoDevolvido = emp != null 
                && (emp.Any(x => x.DataDevolucao.Date.Subtract(diaHoje).Days < 0 && x.StatusEmprestimo == Enums.EnumEmprestimoStatus.EmAberto));

            return possuiEmprestimoNaoDevolvido;
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
                    var status = fazRegistroNovo ? HttpStatusCode.Created : HttpStatusCode.OK;
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
            }
        }

        public void AtualizaQuantidadeLivrosEmprestados(Emprestimo emprestimo, bool fazRegistroNovo)
        {
            if (fazRegistroNovo)
            {
                emprestimo.ItensEmprestimos.ToList().ForEach(i =>
                {
                    i.Livro.EmprestaLivro();                    
                });
            }
            else
            {   
                if(emprestimo.StatusEmprestimo == Enums.EnumEmprestimoStatus.Devolvido)
                {
                    foreach (var itensEmprestimo in emprestimo.ItensEmprestimos.ToList())
                    {                        
                        if(itensEmprestimo.Livro != null)
                        {
                            itensEmprestimo.Livro.DevolveLivro();
                            Console.WriteLine($"Devolver: {itensEmprestimo.LivroId}");
                        }
                    }
                }
            }
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

        public bool LivroExisteEmAlgumEmprestimo(Livro livro)
        {
            bool existe = false;
            var emprestimos = ((Repository)_repo).GetEmprestimosByLivroIdAsync(livro.Id);

            existe = emprestimos != null && emprestimos.Result.Count > 0;
            return existe;
        }

        /*
            •	Usuário só pode ser apagado caso não tenha nenhum empréstimo em seu nome
            •	Não pode repetir telefone

         */
        public async Task<List<string>> RetornaErrosValidacaoUsuarioNoRegistroAsync(UsuarioDTO usuarioEntrada, bool usuarioNovo = true)
        {
            List<string> listaErros = new List<string>();
            Usuario? usuarioPesquisado = await _repo.GetUsuarioByTelefoneAsync(usuarioEntrada.Telefone);
            string mensagemErro = $"O telefone {usuarioEntrada.Telefone} já pertence a outro usuário.";
            if (usuarioNovo)
            {
                if (usuarioPesquisado != null)
                {
                    listaErros.Add(mensagemErro);
                }

                return listaErros;
            }
            //se for usuário para atualizar continua aqui
            if (usuarioPesquisado != null && usuarioPesquisado.Id != usuarioEntrada.Id)
            {
                listaErros.Add(mensagemErro);
            }

             return listaErros;
        }

        public List<string> RetornaErrosValidacaoUsuarioNoDelete(Usuario usuarioDeletar)
        {
            List<string> listaErros = new List<string>();
            int usuarioId = usuarioDeletar.Id;
            var emp = _repo.GetEmprestimosByUsuarioIdAsync(usuarioId);            

            if (emp != null && emp.Result.Count > 0)
            {
                string mensagemErro = $"Não é possível excluir usuário id: {usuarioId}. Possui empréstimo cadastrado.";
                listaErros.Add(mensagemErro);
            }

            return listaErros;

        }
    }
}
