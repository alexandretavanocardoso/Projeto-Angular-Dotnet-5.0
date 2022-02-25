import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import {
  AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';

import { EventoService } from '@app/services/Evento.service';
import { LoteService } from '@app/services/lote.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-EventoDetalhe',
  templateUrl: './EventoDetalhe.component.html',
  styleUrls: ['./EventoDetalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  imagemURL = '../../../../assets/download.jpg'
  file!: File[];

  modalRef!: BsModalRef;

  evento!: Evento;
  eventoId!: number;

  loteAtual = {id: 0, nome: " ", indice: 0};

  form!: FormGroup;
  locale: string = 'pt-br';
  estadoSalvar: string = 'post';

  get modoEditar(): boolean{
    return this.estadoSalvar == 'put';
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  get bsConfigLote(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  get f(): any {
    return this.form.controls;
  }

  get lotes(): FormArray{
    return this.form.get('lotes') as FormArray;
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activedRouter: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router,
    private modalService: BsModalService
  ) {
    this.localeService.use(this.locale);
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();
  }

  private validation(): void {
    this.form! = this.fb.group({
      temaEvento: [
        '',
        [
          Validators.required,
          Validators.minLength(4),
          Validators.maxLength(50),
        ],
      ],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      quantidadePessoas: [
        '',
        [Validators.required, Validators.maxLength(120000)],
      ],
      imagemUrl: [''],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
  }

  public resetForm(): void {
    this.form.reset();
    this.router.navigate([`Eventos/EventoLista`]);
  }

  public cssValidator(campoForm: FormControl | AbstractControl | null): any {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched };
  }

  public carregarEvento(): void {
   this.eventoId = Number(this.activedRouter.snapshot.paramMap.get('id'));

    if (this.eventoId != null && this.eventoId != 0) {
      this.spinner.show();

      this.estadoSalvar = 'put';

      this.eventoService.getEventoById(+this.eventoId).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);

          if(this.evento.imagemUrl != ''){
            this.imagemURL = environment.apiUrl + 'Resources/image/' + this.evento.imagemUrl;
          }

          this.carregarLotes();
        },
        error: () => {
          this.spinner.hide();
          this.toastr.error('Erro ao carregar evento', 'Erro!');
        },
        complete: () => {
          this.spinner.hide();
        },
      });
    }
  }

  public salvarEventos(): void {
    this.spinner.show();
    if (this.form.valid) {

      this.evento = (this.estadoSalvar === 'post') ? { ...this.form.value } : this.evento = {id: this.evento.id, ...this.form.value };

      if(this.estadoSalvar === 'post'){
        this.eventoService.post(this.evento).subscribe(
          (eventoRetorno: Evento) => {
            this.toastr.success('Evento salvo', 'Sucesso');
            this.router.navigate([`Eventos/EventoDetalhe/${eventoRetorno.id}`]);

            this.carregarLotes();
          },
          (error: any) => {
            console.error(error);
            this.toastr.error('Erro ao criar!', 'Erro');
          }
        ).add(() => this.spinner.hide());
      } else {
        this.eventoService.put(this.evento).subscribe(
          () => this.toastr.success('Evento salvo', 'Sucesso'),
          (error: any) => {
            console.error(error);
            this.toastr.error('Erro ao criar!', 'Erro');
          }
        ).add(() => this.spinner.hide());
      }
    }
  }

  public adicionarLote(): void {
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  public criarLote(lote: Lote): FormGroup{
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim],
    });
  }

  public carregarLotes(): void {
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {

        lotesRetorno.forEach(lotesFor => {
          this.lotes.push(this.criarLote(lotesFor));
        });

      },
      (error) => {
        this.toastr.error('Erro ao tentar carrgar lotes!', 'Erro');
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  public salvarLotes(): void{
    if(this.form.controls['lotes'].valid){
      this.spinner.show();
      this.loteService.saveLote(this.eventoId, this.form.value.lotes).subscribe(
        () => {
          this.toastr.success("Lotes salvos com sucesso", "Sucesso");

          //
          //this.lotes.reset();
        },
        (error) => {
          this.toastr.error("Erro ao tentar salvar lotes", "Erro");
          console.error(error);
        },
        () => {}
      ).add(() => this.spinner.hide());
    }
  }

  public excluirLote(template: TemplateRef<any>, indice: number): void  {
    this.loteAtual.id = this.lotes.get(indice + '.id')?.value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome')?.value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public confirmRemoveLote(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.toastr.success("Sucesso ao deletar lote", "Sucesso");
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error) => {
        this.toastr.error("Erro ao excluir lote", "Erro");
        console.error(error);
      }
    ).add(() => this.spinner.hide());
  }

  public declineRemoveLote(): void {
      this.modalRef.hide();
  }

  public retornaTituloLote(nome: string): string {
    return nome === null || nome === '' ? 'Nome do lote' : nome
  }

  public onFileChange(ev: any): void{
    const reader = new FileReader();

    // quando carregar o meu reader vai alterar a imagem do this.imagemURL
    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImagem();
  }

  public uploadImagem(): void {
      this.spinner.show();

      this.eventoService.postUpload(this.eventoId, this.file).subscribe(
        () => {
          this.carregarEvento();
          this.toastr.success("Sucesso ao atualizar imagem", "Sucesso");
        },
        (error: any) => {
          this.toastr.success("Erro ao atualizar imagem", "Erro");
          console.error(error);
        }
      ).add(() => this.spinner.hide())
  }
}
