using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.Entities;

[Table("tbUsuario")]
public class Usuario : EntidadePadrao
{
    [Column("nome")]
    [Required(ErrorMessage = "Campo \"nome\" é obrigatório!")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "O campo nome deve ter entre 5 e 100 caracteres!")]
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
