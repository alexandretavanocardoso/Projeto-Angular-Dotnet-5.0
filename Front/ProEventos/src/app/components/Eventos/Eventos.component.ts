
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '../../models/Evento';
import { EventoService } from '../../services/Evento.service';


@Component({
  selector: 'app-Eventos',
  templateUrl: './Eventos.component.html',
  styleUrls: ['./Eventos.component.scss']
})
export class EventosComponent implements OnInit {
  modalRef?: BsModalRef;

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

  constructor(private eventoService: EventoService,
              private modalService: BsModalService,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService) { }

  public ngOnInit() {
    this.load();

    this.getEvetos();
  }

  public getEvetos(): void {
    this.eventoService.getEventos().subscribe({
      next: (_eventos: Evento[]) => {
        this.eventos = _eventos,
        this.eventosFiltrados = this.eventos
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos.', 'Erro!');
      },
      complete: () => this.spinner.hide()
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

  public openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirm(): void {
    this.modalRef?.hide();
    this.toastr.success('O Eventos foi deletado com sucesso.', 'Deletado!');
  }

  public decline(): void {
    this.modalRef?.hide();
  }

  public load() : void{
    this.spinner.show();
  }
}
