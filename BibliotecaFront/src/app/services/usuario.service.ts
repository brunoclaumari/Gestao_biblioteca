import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Usuario } from '../models/Usuario';
import { first, take, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService {

  private readonly BASE_API = "http://localhost:5000/api/Usuario";

  constructor(private httpClient: HttpClient ) { }

  //private getCoursesEndpoint: string = '/courses';
  list(){
    return this.httpClient.get<Usuario[]>((this.BASE_API))
    .pipe(
      //first(),//se os dados forem estáticos usa-se esse método 'first'
      //delay(5000),
      tap(usuarios => console.log(usuarios))
    );
  }

  loadById(id:string){
    return this.httpClient.get<Usuario>((`${this.BASE_API}/${id}`)).pipe(take(1));
  }

  save(retornoForm: Partial<Usuario>){

    debugger
    if(retornoForm.id){
      console.log('update');
      return this.update(retornoForm);
    }

    console.log('create');
    return this.create(retornoForm);
  }

  private create(retornoForm: Partial<Usuario>){
    debugger
    retornoForm.id = '0';
    return this.httpClient.post<Usuario>(this.BASE_API, retornoForm).pipe(first());
  }

  private update(retornoForm: Partial<Usuario>){

    return this.httpClient.put<Usuario>(`${this.BASE_API}/${retornoForm.id}`, retornoForm).pipe(first());
  }

  delete(id: string){

    return this.httpClient.delete(`${this.BASE_API}/${id}`).pipe(first());
  }
}
