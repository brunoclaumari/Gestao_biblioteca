using GestaoBiblioteca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoBiblioteca.DTO;


public class EmprestimoDTOEntrada : EntidadePadraoDTO{

    
    public int UsuarioId { get; set; }

    
    public DateTime DataEmprestimo { get; set; }

    
    public DateTime DataDevolucao { get; set; }

    
    public EnumEmprestimoStatus StatusEmprestimo { get; set; } = EnumEmprestimoStatus.EmAberto;

    public virtual ICollection<ItensEmprestimoDTO> ItensEmprestimos { get; set; } = new List<ItensEmprestimoDTO>();

    
}
