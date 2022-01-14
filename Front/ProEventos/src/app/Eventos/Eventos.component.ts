
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-Eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getEvetos();
  }

  public getEvetos(): void {
    this.http.get('https://localhost:5001/api/eventos/recuperarGet').subscribe(
      response => this.eventos = response,
      error => console.log(error)
    );
  }
}
