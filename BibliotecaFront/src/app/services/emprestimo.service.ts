import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Emprestimo } from '../models/Emprestimo';
import { first, take, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EmprestimoService {

  private readonly BASE_API = "http://localhost:5000/api/Emprestimo";

  constructor(private httpClient: HttpClient ) { }

  list(){
    return this.httpClient.get<Emprestimo[]>((this.BASE_API))
    .pipe(
      //first(),//se os dados forem estáticos usa-se esse método 'first'
      //delay(5000),
      tap(emp => console.log(emp))
    );
  }

  loadById(id:string){
    return this.httpClient.get<Emprestimo>((`${this.BASE_API}/${id}`)).pipe(take(1));
  }


  save(retornoForm: Partial<Emprestimo>){

    debugger
    if(retornoForm.id){
      console.log('update');
      return this.update(retornoForm);
    }

    console.log('create');
    return this.create(retornoForm);
  }

  private create(retornoForm: Partial<Emprestimo>){
    debugger
    retornoForm.id = '0';
    return this.httpClient.post<Emprestimo>(this.BASE_API, retornoForm).pipe(first());
  }

  private update(retornoForm: Partial<Emprestimo>){

    return this.httpClient.put<Emprestimo>(`${this.BASE_API}/${retornoForm.id}`, retornoForm).pipe(first());
  }
}
