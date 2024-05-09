using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.DTO;


public class UsuarioDTO
{

    public int Id { get; set; }

    
    [Required(ErrorMessage = "Campo \"nome\" é obrigatório!")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "O campo nome deve ter entre 5 e 100 caracteres!")]
    public string Nome { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "Campo \"endereço\" é obrigatório!")]
    [StringLength(250, MinimumLength = 5, ErrorMessage = "O campo nome deve ter entre 5 e 250 caracteres!")]
    public string Endereco { get; set; } = string.Empty;

    
    [MaxLength(15)]
    [Required(ErrorMessage = "Campo \"telefone\" é obrigatório!")]
    [RegularExpression(@"^\([1-9]{2}\) 9[0-9]{4}-[0-9]{4}$", ErrorMessage = "O número de telefone celular deve estar no formato (XX) 9XXXX-XXXX")]
    public string Telefone { get; set; } = string.Empty;  
    
}
