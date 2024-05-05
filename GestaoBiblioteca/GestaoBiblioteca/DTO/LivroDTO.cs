using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.DTO;


public class LivroDTO
{
    
    public string Titulo { get; set; } = string.Empty;
    
    public string? Autores { get; set; }
    
    public int? Genero { get; set; }
    
    public int QuantidadeTotal { get; set; }
    
}
