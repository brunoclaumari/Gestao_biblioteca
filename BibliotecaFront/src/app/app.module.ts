import { MatDialog } from '@angular/material/dialog';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavbarComponent } from './navbar/navbar.component';
import { UsuarioComponent } from './components/usuario/usuario/usuario.component';
import { UsuarioListaComponent } from './components/usuario/usuario-lista/usuario-lista.component';
import { UsuarioFormComponent } from './components/usuario/usuario-form/usuario-form.component';
import { LivroComponent } from './components/livro/livro/livro.component';
import { EmprestimoComponent } from './components/emprestimo/emprestimo/emprestimo.component';
import { EmprestimoFormComponent } from './components/emprestimo/emprestimo-form/emprestimo-form.component';
import { EmprestimoListaComponent } from './components/emprestimo/emprestimo-lista/emprestimo-lista.component';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatDialogModule } from '@angular/material/dialog';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';
import { CustomInputComponent } from './components/custom-input/custom-input.component';
import { ToastrModule } from 'ngx-toastr';
import { ConfirmationDialogComponent } from './shared/confirmation-dialog/confirmation-dialog.component';
import { ErrorDialogComponent } from './shared/error-dialog/error-dialog.component';
import {MatTooltipModule} from '@angular/material/tooltip';
import { LivroFormComponent } from './components/livro/livro-form/livro-form.component';
import { LivroListaComponent } from './components/livro/livro-lista/livro-lista.component';
import {MatListModule} from '@angular/material/list';
import {MatDividerModule} from '@angular/material/divider';

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    UsuarioComponent,
    UsuarioListaComponent,
    UsuarioFormComponent,
    LivroComponent,
    LivroFormComponent,
    LivroListaComponent,
    EmprestimoComponent,
    EmprestimoFormComponent,
    EmprestimoListaComponent,
    CustomInputComponent,
    ConfirmationDialogComponent,
    ErrorDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    HttpClientModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatTableModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule,
    MatSelectModule,
    ReactiveFormsModule,
    FormsModule,
    NgxMaskDirective,
    NgxMaskPipe,
    ToastrModule.forRoot({
      preventDuplicates:true,
      positionClass:'toast-top-center',
        timeOut:3000,
        progressBar:true,
        progressAnimation: 'increasing'
    }),
    MatTooltipModule,
    MatDividerModule,
    MatListModule

  ],
  //providers: [],
  providers: [provideNgxMask({ /* opções de cfg */ })],
  bootstrap: [AppComponent]
})
export class AppModule { }
