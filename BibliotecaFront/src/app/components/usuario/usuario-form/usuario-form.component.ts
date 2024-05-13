import { Usuario } from './../../../models/Usuario';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, NonNullableFormBuilder, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, ActivatedRoute } from '@angular/router';

import { UsuarioService } from 'src/app/services/usuario.service';
import {Location} from '@angular/common';
import { catchError, of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-usuario-form',
  templateUrl: './usuario-form.component.html',
  styleUrls: ['./usuario-form.component.scss']

})
export class UsuarioFormComponent implements OnInit {

  mascaraTelefone:RegExp = /^\([1-9]{2}\) 9[0-9]{4}-[0-9]{4}$/;

  selected: String;
  usuario: Usuario = {
    id:'', nome:'',telefone:'',
  dataAtualizacao: '', dataRegistro: new Date().toDateString(), endereco:''};
  public form: FormGroup = new FormGroup({});

  temErros:boolean = false;

  criarForm(){
    this.form = this.formBuilder.group({
      id: [''],
      nome: [
        '',
        [Validators.required, Validators.minLength(5), Validators.maxLength(100)],
      ],
      telefone:     ['', [Validators.required,Validators.pattern(this.mascaraTelefone)]],//
      endereco:       ['', [Validators.required,Validators.minLength(5), Validators.maxLength(250)]],
/*       teste: ['', [Validators.required]],
      dataRegistro:  ['', [Validators.required]],
      dataAtualizacao: ['', [Validators.required]],
      possuiPendencias: ['', [Validators.required]], */
    });
  }

  constructor(
    private formBuilder: FormBuilder,
    private usuarioService: UsuarioService,
    private toastr: ToastrService,
    private snackBar: MatSnackBar,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private location: Location
  ) {
    this.selected = 'option1';
  }

  focoErro(noFoco:boolean){
    this.temErros = noFoco;
  }

  ngOnInit(): void {
    //debugger;
    this.criarForm()
    let id = this.activeRoute.snapshot.params['id'];
    if(id && id != undefined){
      this.getUsuarioEspecifico(id);
    }
  }

  getUsuarioEspecifico(id:string){
    this.usuarioService.loadById(id).subscribe({
      next: (result) => {
        //debugger
        this.usuario = result as Usuario;
        console.log(this.usuario);
        this.updateForm(this.usuario);
      },
      error: (erro) => {
        this.onError("Erro ao carregar usuário!", "Falhou!");
        //alert("Erro ao carregar usuário!");
        console.log(erro)
      }
    });

  }

  updateForm(usuario: Usuario) {
    //debugger
    this.form.patchValue({
      id: usuario.id,
      nome: usuario.nome,
      telefone: usuario.telefone.trim(),
      //dataAtualizacao: usuario.dataAtualizacao,
      //dataRegistro: usuario.dataAtualizacao,
      endereco: usuario.endereco,
    });
  }

  onSubmit() {
    this.focoErro(true);
    //debugger
    console.log(`Form: ${JSON.stringify(this.form.value)}`);
    if(!this.form.valid){
      this.onError("Verifique as mensagens de erro no formulário", "Formulário inválido");
      this.focoErro(true);
      //this.router.navigate(['', {relativeTo:this.activeRoute}]);
      return ;
    }


    this.usuarioService.save(this.form.value).subscribe({
      next: (result) => {
        //debugger
        this.focoErro(false);
        console.log(`Result: ${JSON.stringify(result)}`);
        this.router.navigate(['usuario']); //{relativeTo:this.activeRoute}
        this.onSuccess(this.form.value);
      },
      error: (erro) => {
        this.focoErro(true);
        this.onError('Falha ao salvar dados do usuário !!');
        return of([]);
/*         alert("Erro ao carregar usuário!");
        console.log(erro) */
      }
    });
/*     this.usuarioService
      .save(this.form.value)
      .pipe(
        catchError((erro) => {
          debugger
          this.openSnackBar('Algo deu errado!!', '');
          return of([]);
        })
      )
      .subscribe((result) => {
        debugger
        console.log(`Result: ${JSON.stringify(result)}`);
        this.router.navigate(['usuario']); //{relativeTo:this.activeRoute}
        this.onSuccess(this.form.value);
      }); */
  }

  onSuccess(result: Partial<Usuario>) {
    //msgSuccess = 'Curso salvo com sucesso';
    this.toastr.success(`Usuário "${result.nome}" salvo com sucesso!`, 'Sucesso');
    //this.openSnackBar(`Usuário "${result.nome}" salvo com sucesso! `, '');
  }

  onError(msg:string, titulo:string = ''){
    this.toastr.error(msg, "Ocorreu algum erro");
  }

  openSnackBar(message: string, action: string) {
    this.snackBar.open(message, action, {
      duration: 5000,
      //panelClass: ['green-snackbar','green'],
    });
  }

  onCancel() {
    this.location.back();
  }

/*   getErrorMessage(fieldName: string) {
    const field = this.form.get(fieldName);
    if (field?.hasError('required')) {
      return `Campo "${fieldName}" é obrigatório!`;
    }

    if (field?.hasError('minlength')) {
      const requiredLength:number = field.errors? field.errors['minlength']['requiredLength']:5;
      return `Tamanho mínimo tem que ser ${requiredLength} caracteres!`;
    }

    if (field?.hasError('maxlength')) {
      const requiredLength:number = field.errors? field.errors['maxlength']['requiredLength']:100;
      return `Tamanho máximo tem que ser ${requiredLength} caracteres!`;
    }

    if(fieldName == 'telefone' && field?.hasError('pattern')){
      return `Digite o telefone no formato (99) 99999-9999`;
    }

    return 'Campo inválido';
  } */

}
