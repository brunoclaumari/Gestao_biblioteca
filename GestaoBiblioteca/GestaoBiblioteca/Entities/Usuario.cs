using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.Entities;

[Table("tbUsuario")]
public class Usuario : EntidadePadrao
{
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("endereco")]
    public string Endereco { get; set; } = string.Empty;

    [Column("data_registro")]
    public DateTime DataRegistro { get; set; }

    [Column("data_atualizacao")]
    public DateTime? DataAtualizacao { get; set; }

    [Column("possui_pendencias")]
    [DefaultValue(false)]
    public bool PossuiPendencias { get; set; } = false;

    [JsonIgnore]
    public virtual ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();
}
