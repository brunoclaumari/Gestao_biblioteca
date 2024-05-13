import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsuarioComponent } from './components/usuario/usuario/usuario.component';
import { UsuarioFormComponent } from './components/usuario/usuario-form/usuario-form.component';
import { LivroComponent } from './components/livro/livro/livro.component';
import { LivroFormComponent } from './components/livro/livro-form/livro-form.component';
import { EmprestimoComponent } from './components/emprestimo/emprestimo/emprestimo.component';
import { EmprestimoFormComponent } from './components/emprestimo/emprestimo-form/emprestimo-form.component';


const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'usuario' },
  {
    path: 'usuario', children: [
      { path: '', component: UsuarioComponent },
      { path: 'new', component: UsuarioFormComponent },
      { path: 'edit/:id', component: UsuarioFormComponent },
    ]
  },
  {
    path: 'livro', children: [
      { path: '', component: LivroComponent },
      { path: 'new', component: LivroFormComponent },
      { path: 'edit/:id', component: LivroFormComponent },
    ]
  },
  {
    path: 'emprestimo', children: [
      { path: '', component: EmprestimoComponent },
      { path: 'new', component: EmprestimoFormComponent },
      { path: 'edit/:id', component: UsuarioFormComponent },
    ]

  }
  /*
  loadChildren: () => import('./courses/courses.module').then(m => m.CoursesModule)
  */
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
