import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lote';
import { Observable, take } from 'rxjs';

@Injectable()

export class LoteService {
  baseURL = 'https://localhost:5001/api/lotes';

  constructor(private http: HttpClient) { }

  public getLotesByEventoId(eventoId: number) : Observable<Lote[]>{
    return this.http.get<Lote[]>(`${this.baseURL}/recuperarLotePorId?eventoId=${eventoId}`).pipe(take(1));
  }

  public saveLote(eventoId: number, lotes: Lote[]) : Observable<Lote[]> {
    return this.http.put<Lote[]>(`${this.baseURL}/saveLotes?eventoId=${eventoId}`, lotes).pipe(take(1));
  }

  public deleteLote(eventoId: number, loteId: number): Observable<any> {
    return this.http.delete(`${this.baseURL}/deletarLote?eventoId=${eventoId}&?loteId=${loteId}`).pipe(take(1));
  }
}
