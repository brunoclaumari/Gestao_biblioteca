using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.Entities;

[Table("tbLivro")]
public class Livro : EntidadePadrao
{
    [Column("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [Column("autores")]
    public string? Autores { get; set; }

    [Column("genero")]
    public int? Genero { get; set; }

    [Column("quantidade_total")]
    public int QuantidadeTotal { get; set; }

    [JsonIgnore]
    public virtual ICollection<ItensEmprestimo> ItensEmprestimos { get; set; } = new List<ItensEmprestimo>();
}
