import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, catchError, of } from 'rxjs';
import { Livro } from 'src/app/models/Livro';
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

  toplivro = new FormControl();//[{"id":'',"titulo":''}]

/*   toppings = new FormControl('');

  toppingList: string[] = ['Extra cheese', 'Mushroom', 'Onion', 'Pepperoni', 'Sausage', 'Tomato']; */

constructor(
/*
private router: Router,
private location: Location, */
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
    let nome_usuario = this.activeRoute.snapshot.queryParams['nomeUsuario']
    console.log(`usuario: ${usuarioId}`)
    this.updateUsuario(nome_usuario);
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

  updateUsuario(nome_usuario: string) {
    //debugger
    this.form.patchValue({
      nomeUsuario: nome_usuario
      /* id: usuario.id,
      nome: usuario.nome,
      telefone: usuario.telefone.trim(),
      //dataAtualizacao: usuario.dataAtualizacao,
      //dataRegistro: usuario.dataAtualizacao,
      endereco: usuario.endereco, */
    });
  }

  onSubmit() {

  }

  onCancel(){

  }

}
