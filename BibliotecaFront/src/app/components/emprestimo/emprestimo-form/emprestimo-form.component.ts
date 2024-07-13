import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { Emprestimo } from 'src/app/models/Emprestimo';
import { ItensEmprestimo } from 'src/app/models/ItensEmprestimo';
import { Livro } from 'src/app/models/Livro';
import { Usuario } from 'src/app/models/Usuario';
import { EmprestimoService } from 'src/app/services/emprestimo.service';
import { LivroService } from 'src/app/services/livro.service';


@Component({
  selector: 'app-emprestimo-form',
  templateUrl: './emprestimo-form.component.html',
  styleUrls: ['./emprestimo-form.component.scss']
})
export class EmprestimoFormComponent implements OnInit  {

  livros$: Observable<Livro[]> | null = null;
  public form: FormGroup = new FormGroup({ });

  listaLivros: Livro[]=[];

  temErros:boolean = false;
  nome_usuario:string = '';
  selecaoLivrosInvalidos:boolean = false;

  itensEmprestimos = new FormControl();


/*   toppings = new FormControl('');

  toppingList: string[] = ['Extra cheese', 'Mushroom', 'Onion', 'Pepperoni', 'Sausage', 'Tomato']; */

constructor(
/*
*/
private location: Location,
private router: Router,
private snackBar: MatSnackBar,
  private emprestimoService: EmprestimoService,
  private toastr: ToastrService,
  private livroService: LivroService,
  private formBuilder: FormBuilder,
  private activeRoute: ActivatedRoute,
) {

}

  ngOnInit(): void {
    this.criarForm();
    let usuarioId = this.activeRoute.snapshot.params['usuarioId'];
    this.nome_usuario = this.activeRoute.snapshot.queryParams['nomeUsuario']
    console.log(`usuario: ${usuarioId}`)
    this.updateUsuario(this.nome_usuario, usuarioId);
  }

  criarForm(){
    let dataHoje: Date = new Date();
    let dataEntrega = new Date();
    dataEntrega.setDate(dataHoje.getDate() + 7);
    this.form = this.formBuilder.group({
      id: [''],
      usuarioId:[''],
      nomeUsuario: [{value:'',disable: true} ],
      dataInicioEmprestimo: [dataHoje.toLocaleDateString()],
      dataDevolucaoEmprestimo: [dataEntrega.toLocaleDateString()],
      //toplivro: new FormControl()
    });

    this.form.get('nomeUsuario')?.disable();
    this.form.get('dataInicioEmprestimo')?.disable();
    this.form.get('dataDevolucaoEmprestimo')?.disable();
    this.atualizaListaLivros();
  }

  atualizaListaLivros() {
    this.livros$ = this.livroService.list().pipe(
      catchError((erro) => {

        this.toastr.error('Erro ao carregar livros.');
        return of([]);
      })
    );


    if(this.livros$){
      this.livros$.forEach(lv=>{
        this.listaLivros = lv;
        console.log(this.listaLivros)
        /* lv.map((l, i)=>{

        }) */
      });
    }
  }

  updateUsuario(nome_usuario: string, usuario_id:string) {
    //debugger
    this.form.patchValue({
      nomeUsuario: nome_usuario,
      usuarioId:usuario_id
    });
  }

  onSubmit() {
    //this.focoErro(true);
    debugger
    console.log(`Form: ${JSON.stringify(this.form.value)}`);
    this.selecaoLivrosInvalidos = this.itensEmprestimos.value == null || this.itensEmprestimos.value?.length == 0;
    if(!this.form.valid || this.selecaoLivrosInvalidos){
      this.onError("Verifique as mensagens de erro no formulário", "Formulário inválido");
      //this.focoErro(true);

      return ;
    }

    debugger
    let emprestimoF:Emprestimo = {
      usuarioId: this.form.value['usuarioId'],
      itensEmprestimos: []
    }

    for (let i = 0; i < this.itensEmprestimos.value?.length; i++) {
      let livroObj:ItensEmprestimo = {
        livroId:this.itensEmprestimos.value?.[i]['id']
      };
      emprestimoF.itensEmprestimos?.push(livroObj);
    }

    //this.emprestimoService.save(this.form.value).subscribe({
    this.emprestimoService.save(emprestimoF).subscribe({
      next: (result) => {
        //debugger
        //this.focoErro(false);
        console.log(`Result: ${JSON.stringify(result)}`);
        this.router.navigate(['usuario']); //{relativeTo:this.activeRoute}
        this.onSuccess(this.form.value);
      },
      error: (erro) => {
        //this.focoErro(true);
        this.onError('Falha ao salvar dados para empréstimo !!');
        return of([]);
      }
    });
  }

  selecaoLivro():boolean{
    //debugger
    return this.selecaoLivrosInvalidos = this.itensEmprestimos.value == null || this.itensEmprestimos.value?.length == 0;
  }

  onSuccess(result: Partial<Emprestimo>) {
    this.toastr.success(`Emprestimo do usuário "${this.nome_usuario}" salvo com sucesso!`, 'Sucesso');
  }

  onError(msg:string, titulo:string = ''){
    this.toastr.error(msg, "Ocorreu algum erro");
  }

  onCancel() {
    this.location.back();
  }

}
