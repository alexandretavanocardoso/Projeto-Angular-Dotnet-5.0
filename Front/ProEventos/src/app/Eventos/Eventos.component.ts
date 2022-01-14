
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-Eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.scss']
})
export class EventosComponent implements OnInit {

  exibirImagem: boolean = false;
  public eventos: any = [];
  public eventosFiltrados: any = [];
  private _filtroLista: string = "";

  public get filtroLista(): string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getEvetos();
  }

  public getEvetos(): void {
    this.http.get('https://localhost:5001/api/eventos/recuperarGet').subscribe(
      response => {
        this.eventos = response,
        this.eventosFiltrados = this.eventos
      },
      error => console.log(error)
    );
  }

  public AlterarImgem(){
    this.exibirImagem = !this.exibirImagem;
  }

  public filtrarEventos(filtrarPor: string): any {
    filtrarPor = filtrarPor?.toLocaleLowerCase();

    return this.eventos.filter((evento: { tema: string; local: string; }) =>
      evento.tema?.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local?.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );

  }
}
