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
    [Required(ErrorMessage = "Campo \"endereço\" é obrigatório!")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "O campo nome deve ter entre 5 e 250 caracteres!")]
    public string Endereco { get; set; } = string.Empty;

    [Column("telefone")]
    [MaxLength(15)]
    [Required(ErrorMessage = "Campo \"telefone\" é obrigatório!")]
    [RegularExpression(@"^\([1-9]{2}\) 9[0-9]{4}-[0-9]{4}$", ErrorMessage = "O número de telefone celular deve estar no formato (XX) 9XXXX-XXXX")]
    public string Telefone { get; set; } = string.Empty;

    [Column("data_registro")]
    [JsonIgnore]
    public DateTime DataRegistro { get; set; }

    [Column("data_atualizacao")]
    [JsonIgnore]
    public DateTime? DataAtualizacao { get; set; }

    private bool _possuiPendencias;

    [Column("possui_pendencias")]
    [DefaultValue(false)]
    public bool PossuiPendencias
    {
        get 
        {
            _possuiPendencias = PossuiEmprestimoNaoDevolvidoEmAberto() || PossuiEmprestimoValidoEmAberto();
            return _possuiPendencias;
            //return _possuiPendencias; 
        }
        //set
        //{
        //    _possuiPendencias = PossuiEmprestimoNaoDevolvidoEmAberto() || PossuiEmprestimoValidoEmAberto();
        //}
    }

    [JsonIgnore]
    public virtual ICollection<Emprestimo> Emprestimos { get; set; } = new List<Emprestimo>();

    bool PossuiEmprestimoValidoEmAberto()
    {
        var emp = this.Emprestimos.ToList();
        DateTime diaHoje = DateTime.Now.Date;
        bool possuiEmprestimoAberto = false;

        possuiEmprestimoAberto = emp != null &&
            (emp.Any(x => x.DataDevolucao.Date.Subtract(diaHoje).Days >= 0 && x.StatusEmprestimo == Enums.EnumEmprestimoStatus.EmAberto));

        return possuiEmprestimoAberto;
    }

    public bool PossuiEmprestimoNaoDevolvidoEmAberto()
    {
        var emp = this.Emprestimos.ToList();
        DateTime diaHoje = DateTime.Now.Date;
        bool possuiEmprestimoNaoDevolvido = false;

        possuiEmprestimoNaoDevolvido = emp != null
            && (emp.Any(x => x.DataDevolucao.Date.Subtract(diaHoje).Days < 0 && x.StatusEmprestimo == Enums.EnumEmprestimoStatus.EmAberto));

        return possuiEmprestimoNaoDevolvido;
    }
}
