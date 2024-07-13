import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { Usuario } from 'src/app/models/Usuario';
import { UsuarioService } from 'src/app/services/usuario.service';
import { ConfirmationDialogComponent } from 'src/app/shared/confirmation-dialog/confirmation-dialog.component';
import { ErrorDialogComponent } from 'src/app/shared/error-dialog/error-dialog.component';
import { EmprestimoComponent } from '../../emprestimo/emprestimo/emprestimo.component';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.scss'],
})
export class UsuarioComponent implements OnInit {
  usuarios$: Observable<Usuario[]> | null = null;
  id: Number = 0;

  constructor(
    private usuarioService: UsuarioService,
    public dialog: MatDialog,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private snackBar: MatSnackBar,
    private toastr: ToastrService
  ) {
    this.atualizaLista();
  }

  atualizaLista() {
    this.usuarios$ = this.usuarioService.list().pipe(
      catchError((erro) => {
        //this.onError('Erro ao carregar usuários.');
        this.toastr.error('Erro ao carregar usuários.');
        return of([]);
      })
    );
  }

  onError(errorMsg: string) {
    /* this.dialog.open(ErrorDialogComponent, {
      data: errorMsg,
    }); */
    this.toastr.error(errorMsg,"Houve um erro");
  }

  ngOnInit(): void {}

  onAdd() {
    this.router.navigate(['new'], { relativeTo: this.activeRoute });
  }

  onEdit(usuario: Usuario) {
    this.router.navigate(['edit', usuario.id], {
      relativeTo: this.activeRoute,
    });
  }

  //onNewEmprestimo(usuario: Usuario){
  onNewEmprestimo(usuario: Usuario){
    //debugger
    let usuarioId = usuario.id;
    this.router.navigate(['emprestimo/new2', usuarioId], {queryParams:{ nomeUsuario: usuario.nome}});

  }

  onDelete(usuario: Usuario) {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      data: `Tem certeza que deseja remover o usuário "${usuario.nome}"?`,
    });

    dialogRef.afterClosed().subscribe((result: boolean) => {
      if (result) {
        this.usuarioService
          .delete(usuario.id).subscribe({
            next: (result) => {
              this.atualizaLista();
              this.toastr.success(
                `Usuário ${usuario.nome} removido com sucesso!`,
                'Sucesso'
              );
            },
            error: (erro) => {
              debugger
              var retorno:string = "";
              if(erro.error.listaMensagens){
                retorno = "\n" + erro.error.listaMensagens[0];
              }
              this.onError(`Erro ao tentar remover usuário ${usuario.nome}.\n\n${retorno}`);
              return of([]);
            }
          });
      }
    });
  }
}
