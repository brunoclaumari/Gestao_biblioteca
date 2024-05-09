using GestaoBiblioteca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.DTO;


public class LivroDTOEntrada
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Campo \"título\" é obrigatório!")]
    [StringLength(200, MinimumLength = 5, ErrorMessage = "O campo nome deve ter entre 5 e 200 caracteres!")]
    public string Titulo { get; set; } = string.Empty;
    
    public string? Autores { get; set; }    
    
    public EnumGeneroLivro Genero { get; set; } //Será o número do enum EnumGeneroLivro
    
    public int QuantidadeTotal { get; set; }    

}
