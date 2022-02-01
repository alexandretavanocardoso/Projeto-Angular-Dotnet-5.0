import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-EventoDetalhe',
  templateUrl: './EventoDetalhe.component.html',
  styleUrls: ['./EventoDetalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;

  get f(): any {
    return this.form.controls;
  }

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.validation();
  }

  private validation(): void{
    this.form! = this.fb.group({
      temaEvento: ['',
      [
        Validators.required,
        Validators.minLength(4),
        Validators.maxLength(50)
      ]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      quantidadePessoas: ['',
      [
        Validators.required,
        Validators.maxLength(120000)
      ]],
      imagemUrl: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [
        Validators.required,
        Validators.email
      ]],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

}
