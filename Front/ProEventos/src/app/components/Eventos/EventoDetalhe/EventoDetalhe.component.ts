import { Component, OnInit } from '@angular/core';
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
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-EventoDetalhe',
  templateUrl: './EventoDetalhe.component.html',
  styleUrls: ['./EventoDetalhe.component.scss'],
})
export class EventoDetalheComponent implements OnInit {
  evento!: Evento;

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
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private router: Router
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
      imagemUrl: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
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

  public resetForm(): void {
    this.form.reset();
    this.router.navigate([`Eventos/EventoLista`]);
  }

  public cssValidator(campoForm: FormControl | AbstractControl | null): any {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched };
  }

  public carregarEvento(): void {
    const eventoIdParam = this.activedRouter.snapshot.paramMap.get('id');

    if (eventoIdParam != null) {
      this.spinner.show();

      this.estadoSalvar = 'put';

      this.eventoService.getEventoById(+eventoIdParam).subscribe({
        next: (evento: Evento) => {
          this.evento = { ...evento };
          this.form.patchValue(this.evento);
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

  public salvarAlteracao(): void {
    this.spinner.show();
    if (this.form.valid) {

      this.evento = (this.estadoSalvar === 'post') ? { ...this.form.value } : this.evento = {id: this.evento.id, ...this.form.value };

      if(this.estadoSalvar === 'post'){
        this.eventoService.post(this.evento).subscribe(
          (eventoRetorno: Evento) => {
            this.toastr.success('Evento salvo', 'Sucesso');

            this.router.navigate([`Eventos/EventoDetalhe/${eventoRetorno.id}`]);
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
}
