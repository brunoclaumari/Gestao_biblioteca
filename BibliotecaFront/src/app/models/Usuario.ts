export interface Usuario {
  id:string;
  nome:string;
  endereco:string;
  telefone:string;
  dataRegistro:string;
  dataAtualizacao:string;
  possuiPendencias?:boolean;
}


/*

public string Nome { get; set; } = string.Empty;


public string Endereco { get; set; } = string.Empty;


[RegularExpression(@"^\([1-9]{2}\) 9[0-9]{4}-[0-9]{4}$", ErrorMessage = "O número de telefone celular deve estar no formato (XX) 9XXXX-XXXX")]
public string Telefone { get; set; } = string.Empty;

[Column("data_registro")]
[JsonIgnore]
public DateTime DataRegistro { get; set; }

[Column("data_atualizacao")]
[JsonIgnore]
public DateTime? DataAtualizacao { get; set; }

[Column("possui_pendencias")]
[DefaultValue(false)]
public bool PossuiPendencias { get; set; } = false;

*/
