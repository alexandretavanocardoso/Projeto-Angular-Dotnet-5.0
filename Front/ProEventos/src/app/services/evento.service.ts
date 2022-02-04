import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable()
export class EventoService {
  baseURL : string = 'https://localhost:5001/api/eventos';

  constructor(private http: HttpClient) { }


  public getEventos() : Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/recuperarTodosEventos`);
  }

  public getEventosByTema(tema: string) : Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/recuperarEventoPorTema?tema=${tema}`);
  }

  public getEventoById(id: number) : Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/recuperarEventoPorId?id=${id}`);
  }

  public postEventos(evento: Evento) : Observable<Evento> {
    return this.http.post<Evento>(`${this.baseURL}/adicionarEvento`, evento);
  }

  public putEventos(id: number, evento: Evento) : Observable<Evento> {
    return this.http.put<Evento>(`${this.baseURL}/alterarEvento?id=${id}`, evento);
  }

  public deleteEventos(id: number) : Observable<string> {
    return this.http.delete<string>(`${this.baseURL}/deletarEvento?id=${id}`);
  }
}
