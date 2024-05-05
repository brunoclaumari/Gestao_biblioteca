using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoBiblioteca.Entities
{
    [Table("tbAutor")]
    public class Autor : EntidadePadrao
    {
        public Autor() { }

        public string Nome { get; set; }

        [Column("autor_id")]
        public string AutorId { get; set; }
    }
}
