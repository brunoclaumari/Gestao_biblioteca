

using GestaoBiblioteca.Entities;

namespace GestaoBiblioteca.DTO
{
    public class CustomResponse
    {
        public int StatusCode { get; set; }

        public bool FoiSucesso { get; set; }

        public EntidadePadrao? Entidade { get; set; }

        public List<string> ListaMensagens { get; set; } = new List<string>();
    }
}
