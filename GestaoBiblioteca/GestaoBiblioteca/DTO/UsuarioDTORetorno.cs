using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.DTO;


public class UsuarioDTORetorno : UsuarioDTO
{    
    
    public DateTime DataRegistro { get; set; }    
    
    public DateTime? DataAtualizacao { get; set; }    
    
}
