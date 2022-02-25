import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, take } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable()
export class EventoService {
  baseURL : string = environment.apiUrl + 'api/eventos';

  constructor(private http: HttpClient) { }

  /*
    take(Numero de vezes que o método vai ser chamado)
  */

  public getEventos() : Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/recuperarTodosEventos`).pipe(take(1));
  }

  public getEventosByTema(tema: string) : Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/recuperarEventoPorTema?tema=${tema}`).pipe(take(1));
  }

  public getEventoById(id: number) : Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/recuperarEventoPorId?id=${id}`).pipe(take(1));
  }

  public post(evento: Evento) : Observable<Evento> {
    return this.http.post<Evento>(`${this.baseURL}/adicionarEvento`, evento).pipe(take(1));
  }

  public put(evento: Evento) : Observable<Evento> {
    return this.http.put<Evento>(`${this.baseURL}/alterarEvento?id=${evento.id}`, evento).pipe(take(1));
  }

  public deleteEventos(id: number): Observable<any> {
    return this.http.delete(`${this.baseURL}/deletarEvento?id=${id}`).pipe(take(1));
  }

  public postUpload(eventoId: number, file: File[]) : Observable<Evento> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file', fileToUpload);

    return this.http.post<Evento>(`${this.baseURL}/uploadImage?eventoId=${eventoId}`, formData).pipe(take(1));
  }
}
