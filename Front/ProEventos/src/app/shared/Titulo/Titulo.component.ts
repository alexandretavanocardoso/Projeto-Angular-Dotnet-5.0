import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-Titulo',
  templateUrl: './Titulo.component.html',
  styleUrls: ['./Titulo.component.scss']
})
export class TituloComponent implements OnInit {

  @Input() titulo: string = "";
  @Input() subtitulo: string = "Desde 2021";
  @Input() icone: string = "fas fa-user";
  @Input() botaoListar: boolean = false;

  constructor(private router: Router) { }

  ngOnInit() : void {
  }


  public listar(): void {
    this.router.navigate([`/${this.titulo}/EventoLista`]);
  }
}
