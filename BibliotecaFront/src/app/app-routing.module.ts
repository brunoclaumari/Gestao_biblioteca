import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsuarioComponent } from './components/usuario/usuario/usuario.component';
import { UsuarioFormComponent } from './components/usuario/usuario-form/usuario-form.component';
import { LivroComponent } from './components/livro/livro/livro.component';
import { LivroFormComponent } from './components/livro/livro-form/livro-form.component';
import { EmprestimoComponent } from './components/emprestimo/emprestimo/emprestimo.component';
import { EmprestimoFormComponent } from './components/emprestimo/emprestimo-form/emprestimo-form.component';


const routes: Routes = [
  /* { path: 'usuario', pathMatch: 'full', component: UsuarioComponent },
  { path: 'usuario/new', pathMatch: 'full', component: UsuarioFormComponent },
  { path: 'usuario/edit/:id', pathMatch: 'full', component: UsuarioFormComponent },
  { path: 'usuario/newEmprestimo',  component: EmprestimoFormComponent },
  { path: 'livro', pathMatch: 'full', component: LivroComponent },
  { path: 'livro/new', pathMatch: 'full', component: LivroFormComponent },
  { path: 'livro/edit/:id', pathMatch: 'full', component: LivroFormComponent },
  { path: 'emprestimo',  pathMatch: 'full', component: EmprestimoComponent },
  { path: 'emprestimo/edit/:id', pathMatch: 'full', component: EmprestimoFormComponent },
  { path: '', pathMatch: 'full', redirectTo: 'usuario' }, */

  { path: '', pathMatch: 'full', redirectTo: 'usuario' },
  {
    path: 'usuario',  children: [
      { path: '', component: UsuarioComponent },
      //{ path: 'novoEmprestimo',  component: EmprestimoFormComponent },
      { path: 'new', component: UsuarioFormComponent },
      { path: 'edit/:id', component: UsuarioFormComponent },
    ]
  },
  {
    path: 'livro', children: [
      { path: '',  component: LivroComponent },
      { path: 'new',  component: LivroFormComponent },
      { path: 'edit/:id',  component: LivroFormComponent },
    ]
  },
  {
    path: 'emprestimo', children: [
      { path: 'new2/:usuarioId', component: EmprestimoFormComponent },
      { path: 'edit/:id', component: EmprestimoFormComponent },
      { path: '',  component: EmprestimoComponent },
      //{ path: 'new/:usuarioId', component: EmprestimoFormComponent },
    ]
  }

];

@NgModule({
  imports: [
    RouterModule.forRoot(routes),
    //RouterModule.forChild(routes)
  ],//forRoot(routes)
  exports: [RouterModule],
})
export class AppRoutingModule {}
