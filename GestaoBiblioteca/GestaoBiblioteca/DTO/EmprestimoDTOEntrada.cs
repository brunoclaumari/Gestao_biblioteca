using GestaoBiblioteca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoBiblioteca.DTO;


public class EmprestimoDTOEntrada : EmprestimoPadraoDTO
{
    
    public int UsuarioId { get; set; }

    public virtual ICollection<ItensEmprestimoDTO> ItensEmprestimos { get; set; } = new List<ItensEmprestimoDTO>();


}
