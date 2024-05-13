import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Usuario } from 'src/app/models/Usuario';

@Component({
  selector: 'app-usuario-lista',
  templateUrl: './usuario-lista.component.html',
  styleUrls: ['./usuario-lista.component.scss']
})
export class UsuarioListaComponent implements OnInit{

  @Input() usuarios: Usuario[] = [];
  @Output() id: Number = 0;
  @Output() add = new EventEmitter(false);
  @Output() edit = new EventEmitter(false);
  @Output() delete = new EventEmitter(false);

  readonly displayedColumns = ['nome', 'telefone','dataRegistro', 'actions'];

  constructor( ){}

  ngOnInit(): void {
  }

  onAdd(){
    //console.log('onAdd');
    this.add.emit(true);
    //this.router.navigate(['new'], {relativeTo:this.activeRoute})
  }

  onEdit(usuario:Usuario){
    //debugger
    this.edit.emit(usuario);
    //this.router.navigate(['update',_id], {queryParams: {name: curso.name, category: curso.category}, relativeTo:this.activeRoute})
  }

  onDelete(usuario:Usuario){
    this.delete.emit(usuario);
  }

}
