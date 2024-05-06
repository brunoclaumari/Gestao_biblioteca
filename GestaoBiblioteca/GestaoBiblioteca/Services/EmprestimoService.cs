using GestaoBiblioteca.Context;
using GestaoBiblioteca.DTO;
using GestaoBiblioteca.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace GestaoBiblioteca.Services
{
    public class EmprestimoService
    {

        private readonly GestaoBibliotecaContext _context;

        public EmprestimoService(GestaoBibliotecaContext context) {
            //context = new GestaoBibliotecaContext();
            _context = context;
        }

        /// <summary>
        /// Faz verificações do usuário para ver se ele é válido
        /// </summary>
        /// <param name="emprestimo"></param>
        /// <returns></returns>
        public string VerificaEmprestimoValido(Emprestimo emprestimo)
        {
            /*
             Um empréstimo de livro só pode ser permitido se:
              * O usuário estiver cadastrado no banco
              * Se o usuário não possui pendências
              * O livro estiver cadastrado no banco
              * Possuir livros disponíveis
             */

            string mensagemErro = string.Empty;

            int usuarioId = emprestimo.UsuarioId;
            var usuario = _context.Usuarios.FirstOrDefault(x=>x.Id == usuarioId);
            if (usuario == null)
            {
                mensagemErro = $"Não existe usuário com o id: {usuarioId} fornecido";
            }
            else
            {
               var emprestimosPendentes =  _context.Emprestimos.ToList().FindAll(x => x.DataDevolucao < DateTime.Now);
                if(emprestimosPendentes != null && emprestimosPendentes.Count > 0)
                {
                    mensagemErro = $"Usuário id: {usuarioId} possui empréstimos pendentes. Não é permitido novos empréstimos";
                }
            }

            return mensagemErro;

        }

        private bool LivroExiste(Emprestimo emprestimo)
        {
            bool existe = false;
            //existe = _context.Livros.Any(lv => lv.Id == emprestimo.ItensEmprestimos.Any(i=>i.LivroId == lv.Id));

            return existe;
        }

        public Emprestimo ConverteDTOParaEmprestimo(EmprestimoDTOEntrada dto)
        {
            List<ItensEmprestimo> itensEmprestimos = new List<ItensEmprestimo>();
            dto.ItensEmprestimos.ToList().ForEach(i =>
            {
                ItensEmprestimo item = new ItensEmprestimo
                {
                    LivroId = i.LivroId,
                    EmprestimoId = i.EmprestimoId,    
                };
                itensEmprestimos.Add(item);
                //i.LivroId
            });
            Emprestimo emprestimo = new Emprestimo()
            {
                DataEmprestimo = dto.DataEmprestimo,
                DataDevolucao = dto.DataDevolucao,
                ItensEmprestimos = itensEmprestimos,
                StatusEmprestimo = dto.StatusEmprestimo,
                UsuarioId = dto.UsuarioId,                
            };

            return emprestimo;
        }

        //public static bool VerificaTabelaExiste(string nomeTabela, GestaoBibliotecaContext contextParam)
        //{
        //    bool tabelaExiste = false;
        //    //DbContextOptions options = new();
        //    var serviceProvider = new ServiceCollection()
        //        .AddEntityFrameworkSqlServer()
        //        .BuildServiceProvider();
            
        //    serviceProvider.GetRequiredService<GestaoBibliotecaContext>();
        //    //using (GestaoBibliotecaContext context = new GestaoBibliotecaContext(serviceProvider.GetRequiredService<DbContextOptions<GestaoBibliotecaContext>>()))
        //    //using (GestaoBibliotecaContext context = new GestaoBibliotecaContext()) 
        //    //using (GestaoBibliotecaContext context = contextParam)
        //    //{
        //    //    string sql = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{nomeTabela}'";
        //    //    tabelaExiste = context.Database.ExecuteSqlRaw(sql) > 0;
        //    //}
        //    string sql = $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{nomeTabela}'";
        //    tabelaExiste = contextParam.Database.ExecuteSqlRaw(sql) > 0;

        //    return tabelaExiste;
        //}

        //public static bool IndiceExisteNaTabela(string nomeTabela, string nomeIndice, GestaoBibliotecaContext contextParam)
        //{
        //    bool existe = false;
        //    //SELECT COUNT(name) FROM sys.indexes WHERE object_id = OBJECT_ID(N'tbEmprestimo') AND name = N'IX_tbItensEmprestimo_emprestimo_id'
        //    var serviceProvider = new ServiceCollection()
        //        .AddEntityFrameworkSqlServer()
        //        .BuildServiceProvider();
        //    //using (GestaoBibliotecaContext context = new GestaoBibliotecaContext(serviceProvider.GetRequiredService<DbContextOptions<GestaoBibliotecaContext>>()))
        //    //using (GestaoBibliotecaContext context = new GestaoBibliotecaContext())
        //    //using (GestaoBibliotecaContext context = contextParam)
        //    //{

        //    //    string sql = $"SELECT COUNT(name) FROM sys.indexes WHERE object_id = OBJECT_ID(N'{nomeTabela}') AND name = N'{nomeIndice}';";
        //    //    existe = context.Database.ExecuteSqlRaw(sql) > 0;
        //    //}
        //    string sql = $"SELECT COUNT(name) FROM sys.indexes WHERE object_id = OBJECT_ID(N'{nomeTabela}') AND name = N'{nomeIndice}';";
        //    existe = contextParam.Database.ExecuteSqlRaw(sql) > 0;

        //    return existe;

        //}
    }
}
