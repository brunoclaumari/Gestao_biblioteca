import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Livro } from '../models/Livro';
import { first, take, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LivroService {



  private readonly BASE_API = "http://localhost:5000/api/Livro";

  constructor(private httpClient: HttpClient ) { }

  list(){
    return this.httpClient.get<Livro[]>((this.BASE_API))
    .pipe(
      tap(livros => console.log(livros))
    );
  }

  loadById(id:string){
    return this.httpClient.get<Livro>((`${this.BASE_API}/${id}`)).pipe(take(1));
  }

  save(retornoForm: Partial<Livro>){

    debugger
    if(retornoForm.id){
      console.log('update');
      return this.update(retornoForm);
    }

    console.log('create');
    return this.create(retornoForm);
  }

  private create(retornoForm: Partial<Livro>){
    //debugger
    retornoForm.id = '0';
    return this.httpClient.post<Livro>(this.BASE_API, retornoForm).pipe(first());
  }

  private update(retornoForm: Partial<Livro>){

    return this.httpClient.put<Livro>(`${this.BASE_API}/${retornoForm.id}`, retornoForm).pipe(first());
  }

  delete(id: string){

    return this.httpClient.delete(`${this.BASE_API}/${id}`).pipe(first());
  }

}
