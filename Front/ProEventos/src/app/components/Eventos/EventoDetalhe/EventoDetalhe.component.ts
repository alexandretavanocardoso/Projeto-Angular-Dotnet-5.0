import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';

import { Evento } from '@app/models/Evento';

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

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService
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
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched };
  }

  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');

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
          () => this.toastr.success('Evento salvo', 'Sucesso'),
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
