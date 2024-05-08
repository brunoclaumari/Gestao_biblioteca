using GestaoBiblioteca.Entities;

namespace GestaoBiblioteca.Helpers
{
    /// <summary>
    /// Classe para atualizar quantidades de livro
    /// </summary>
    public static class LivroHelper
    {
        public static void Empresta(Livro livro)
        {
            if(livro is not null)
            {
                livro.EmprestaLivro();
            }
        }

        public static void Devolve(Livro livro)
        {
            if (livro is not null)
            {
                livro.DevolveLivro();
            }
        }
    }
}
