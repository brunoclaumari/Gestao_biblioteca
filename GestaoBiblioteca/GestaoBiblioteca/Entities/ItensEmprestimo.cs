using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.Entities;

[Table("tbItensEmprestimo")]
public class ItensEmprestimo : EntidadePadrao
{
    [Column("livro_id")]
    public int LivroId { get; set; }

    [Column("emprestimo_id")]
    public int EmprestimoId { get; set; }

    [JsonIgnore]
    public virtual Emprestimo Emprestimo { get; set; } = null!;

    public virtual Livro Livro { get; set; } = null!;


}
