import { EnumEmprestimoStatus } from './EnumEmprestimoStatus';
import { ItensEmprestimo } from './ItensEmprestimo';
import { Usuario } from './Usuario';
/*

public int UsuarioId { get; set; }

public DateTime DataEmprestimo { get; set; }

public DateTime DataDevolucao { get; set; }

public EnumEmprestimoStatus StatusEmprestimo { get; set; } = EnumEmprestimoStatus.EmAberto;

public virtual ICollection<ItensEmprestimo> ItensEmprestimos { get; set; } = new List<ItensEmprestimo>();

public virtual Usuario Usuario { get; set; } = null!;
*/

export interface Emprestimo {
  id?:string;
  usuarioId?:string;
  usuario?:Usuario;
  dataEmprestimo?:Date;
  dataDevolucao?:Date;
  statusEmprestimo?:EnumEmprestimoStatus;
  itensEmprestimos?:ItensEmprestimo[];
}
