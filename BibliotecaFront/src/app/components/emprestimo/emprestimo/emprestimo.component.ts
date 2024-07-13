import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { Emprestimo } from 'src/app/models/Emprestimo';
import { EnumEmprestimoStatus } from 'src/app/models/EnumEmprestimoStatus';
import { Usuario } from 'src/app/models/Usuario';
import { EmprestimoService } from 'src/app/services/emprestimo.service';

@Component({
  selector: 'app-emprestimo',
  templateUrl: './emprestimo.component.html',
  styleUrls: ['./emprestimo.component.scss']
})
export class EmprestimoComponent {

  emprestimos$: Observable<Emprestimo[]> | null = null;

  constructor(
    private emprestimoService:EmprestimoService,
    private toastr: ToastrService,
    private router:Router
  ) {
    this.atualizaLista();
    console.log(this.emprestimos$);
  }

  atualizaLista() {
    this.emprestimos$ = this.emprestimoService.list().pipe(
      catchError((erro) => {
        //this.onError('Erro ao carregar usuários.');
        this.toastr.error('Erro ao carregar empréstimos.');
        return of([]);
      })
    );


  }

  onAdd(){}

  onEdit(emprestimo:Emprestimo){

    emprestimo.statusEmprestimo = EnumEmprestimoStatus.Devolvido;
    debugger
    this.emprestimoService.save(emprestimo).subscribe({
      next: (result) => {
        //debugger
        //this.focoErro(false);
        console.log(`Result: ${JSON.stringify(result)}`);
        this.router.navigate(['emprestimo']); //{relativeTo:this.activeRoute}
        this.onSuccess(emprestimo);
      },
      error: (erro) => {
        //this.focoErro(true);
        this.onError('Falha ao salvar dados do usuário !!');
        return of([]);
      }
    });

  }

  onDelete(emprestimo:Emprestimo){}

  onSuccess(result: Partial<Emprestimo>) {
    //let usuario:Usuario = result.usuario;
    this.toastr.success(`Emprestimo do usuário "${result.usuario?.nome}" salvo com sucesso!`, 'Sucesso');
  }

  onError(msg:string, titulo:string = ''){
    this.toastr.error(msg, "Ocorreu algum erro");
  }

}
