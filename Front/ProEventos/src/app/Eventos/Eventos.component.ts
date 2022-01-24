
import { Component, OnInit } from '@angular/core';
import { Evento } from '../models/Evento';
import { EventoService } from '../services/Evento.service';

@Component({
  selector: 'app-Eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  exibirImagem = false;
  private _filtroLista = "";

  public get filtroLista(): string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  constructor(private eventoService: EventoService) { }

  public ngOnInit() {
    this.getEvetos();
  }

  public getEvetos(): void {
    this.eventoService.getEventos().subscribe({
      next: (_eventos: Evento[]) => {
        this.eventos = _eventos,
        this.eventosFiltrados = this.eventos
      },
      error: (error: any) => console.log(error)
    });
  }

  public AlterarImgem(){
    this.exibirImagem = !this.exibirImagem;
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor?.toLocaleLowerCase();

    return this.eventos.filter(evento =>
      evento.temaEvento?.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local?.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );

  }
}
