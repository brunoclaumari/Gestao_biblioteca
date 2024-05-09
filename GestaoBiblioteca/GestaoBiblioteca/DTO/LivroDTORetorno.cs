using GestaoBiblioteca.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GestaoBiblioteca.DTO;


public class LivroDTORetorno : LivroDTOEntrada
{    

    public int QuantidadeEmprestada { get; set; }

}
