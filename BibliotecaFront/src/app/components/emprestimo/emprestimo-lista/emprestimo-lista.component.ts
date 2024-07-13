import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Emprestimo } from 'src/app/models/Emprestimo';

@Component({
  selector: 'app-emprestimo-lista',
  templateUrl: './emprestimo-lista.component.html',
  styleUrls: ['./emprestimo-lista.component.scss']
})
export class EmprestimoListaComponent implements OnInit {

  @Input() emprestimos: Emprestimo[] = [];

  @Output() id: Number = 0;
  //@Output() add = new EventEmitter(false);
  @Output() edit = new EventEmitter(false);
  @Output() delete = new EventEmitter(false);

  readonly displayedColumns = ['id','usuario','dataDevolucao','dataEmprestimo','status','actions'];
  //readonly displayedColumns = ['id', 'Data de empréstimo ','Data de devolução','status', 'actions'];

  constructor(
    private router:Router,
    //private emprestimo:Emprestimo
  ) { }

  ngOnInit(): void{

  }

  onAdd(){
    //this.add.emit(true);
  }

  onEdit(emprestimo:Emprestimo){
    this.edit.emit(emprestimo);
  }

  onDelete(emprestimo:Emprestimo){
    this.delete.emit(emprestimo);
  }

}
