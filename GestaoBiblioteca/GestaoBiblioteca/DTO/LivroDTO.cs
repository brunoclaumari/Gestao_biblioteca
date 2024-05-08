using GestaoBiblioteca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.DTO;


public class LivroDTO
{
    
    public string Titulo { get; set; } = string.Empty;
    
    public string? Autores { get; set; }    
    
    public EnumGeneroLivro Genero { get; set; } //Será o número do enum EnumGeneroLivro
    
    public int QuantidadeTotal { get; set; }

    public int QuantidadeEmprestada { get; set; } = 0;

}
