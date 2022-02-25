import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from '@app/services/Evento.service';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-EventoListagem',
  templateUrl: './EventoListagem.component.html',
  styleUrls: ['./EventoListagem.component.scss'],
})
export class EventoListagemComponent implements OnInit {
  modalRef?: BsModalRef;

  public eventoId: number = 0;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  exibirImagem = false;
  private _filtroLista = '';

  public get filtroLista(): string {
    return this._filtroLista;
  }

  public set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista
      ? this.filtrarEventos(this.filtroLista)
      : this.eventos;
  }

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public ngOnInit() {
    this.load();

    this.getEvetos();
  }

  public getEvetos(): void {
    this.eventoService.getEventos().subscribe({
      next: (_eventos: Evento[]) => {
        (this.eventos = _eventos), (this.eventosFiltrados = this.eventos);
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos.', 'Erro!');
      },
      complete: () => this.spinner.hide(),
    });
  }

  public AlterarImgem() {
    this.exibirImagem = !this.exibirImagem;
  }

  public filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor?.toLocaleLowerCase();

    return this.eventos.filter(
      (evento) =>
        evento.temaEvento?.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local?.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  public openModal(event: any, template: TemplateRef<any>, eventId: number) {
    event.stopPropagation();
    this.eventoId = eventId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  public confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService.deleteEventos(this.eventoId).subscribe(
      () => {
        this.toastr.success('O Eventos foi deletado com sucesso.', 'Deletado!');
        this.getEvetos();
      },
      (error: any) => {
        console.error(error);
        this.toastr.error('Erro o deletar!', 'Erro');
      }
    ).add(() => this.spinner.hide());
  }

  public decline(): void {
    this.modalRef?.hide();
  }

  public load(): void {
    this.spinner.show();
  }

  public detalheEventos(id: number): void {
    this.router.navigate([`/Eventos/EventoDetalhe/${id}`]);
  }

  public retornaImagem(img: string): string {
    return (img != '') ? `${environment.apiUrl}Resources/image/${img}` : 'assets/semImagem.png';
  }
}
