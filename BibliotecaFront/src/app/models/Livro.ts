import { EnumGeneroLivro } from "./EnumGeneroLivro";

export interface Livro {
  id:string;
  titulo:string;
  autores:string;
  genero:EnumGeneroLivro;
  quantidade_total: Number;
  quantidade_emprestada: Number;
}

/*


[Column("genero")]
public EnumGeneroLivro Genero { get; set; } //Será o número do enum EnumGeneroLivro

[Column("quantidade_total")]
public int QuantidadeTotal { get; set; }

//quantidade_emprestada
[Column("quantidade_emprestada")]
[DefaultValue(0)]
public int QuantidadeEmprestada { get; internal set; } = 0;

*/
