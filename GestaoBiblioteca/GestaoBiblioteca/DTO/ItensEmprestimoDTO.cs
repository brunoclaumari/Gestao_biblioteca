using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.DTO;

[Table("tbItensEmprestimo")]
public class ItensEmprestimoDTO
{
    
    public int LivroId { get; set; }

    
    public int EmprestimoId { get; set; }
    
}
