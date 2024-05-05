using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoBiblioteca.Entities;

[Table("tbEmprestimo")]
public class Emprestimo : EntidadePadrao{

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("data_emprestimo")]
    public DateTime DataEmprestimo { get; set; }

    [Column("data_devolucao")]
    public DateTime DataDevolucao { get; set; }

    /// <summary>
    /// <para>0 = Em aberto</para>
    /// <para>1 = Devolvido</para>
    /// </summary>
    [Column("status_emprestimo")]
    public bool StatusEmprestimo { get; set; }

    public virtual ICollection<ItensEmprestimo> ItensEmprestimos { get; set; } = new List<ItensEmprestimo>();

    public virtual Usuario Usuario { get; set; } = null!;
}
