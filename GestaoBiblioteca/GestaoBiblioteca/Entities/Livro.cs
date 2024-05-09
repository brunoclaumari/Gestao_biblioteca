using GestaoBiblioteca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.Entities;

[Table("tbLivro")]
public class Livro : EntidadePadrao
{
    [Column("titulo")]
    [Required(ErrorMessage = "Campo \"título\" é obrigatório!")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "O campo nome deve ter entre 5 e 200 caracteres!")]
    public string Titulo { get; set; } = string.Empty;

    [Column("autores")]
    public string? Autores { get; set; }

    [Column("genero")]
    public EnumGeneroLivro Genero { get; set; } //Será o número do enum EnumGeneroLivro

    [Column("quantidade_total")]
    public int QuantidadeTotal { get; set; }

    //quantidade_emprestada
    [Column("quantidade_emprestada")]
    [DefaultValue(0)]
    public int QuantidadeEmprestada { get; internal set; } = 0;

    [JsonIgnore]
    public virtual ICollection<ItensEmprestimo> ItensEmprestimos { get; set; } = new List<ItensEmprestimo>();


    public override string ToString()
    {
        return $"Livro id: {Id}, título: {Titulo}";
    }

    
    public void EmprestaLivro()
    {
        QuantidadeEmprestada ++;
    }

    public void DevolveLivro()
    {
        QuantidadeEmprestada --;
    }

    public int RetornaQuantidadeDisponivel()
    {
        return QuantidadeTotal - QuantidadeEmprestada;
    }
}
