import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ContatosComponent } from './components/Contatos/Contatos.component';

import { DashboardComponent } from './components/Dashboard/Dashboard.component';

import { EventoDetalheComponent } from './components/Eventos/EventoDetalhe/EventoDetalhe.component';
import { EventoListagemComponent } from './components/Eventos/EventoListagem/EventoListagem.component';
import { EventosComponent } from './components/Eventos/Eventos.component';

import { PalestrantesComponent } from './components/Palestrantes/Palestrantes.component';

import { PerfilComponent } from './components/user/Perfil/Perfil.component';

import { LoginComponent } from './components/user/login/login.component';
import { RegistrationComponent } from './components/user/registration/registration.component';
import { UserComponent } from './components/user/user.component';

const routes: Routes = [
  {
    path: 'user', component: UserComponent,
    children: [
      {path: 'login', component: LoginComponent},
      {path: 'registration', component: RegistrationComponent},
    ],
  },
  {
    path: 'user/Perfil', component: PerfilComponent
  },
  {path: 'Eventos', redirectTo: 'Eventos/EventoLista'},
  {
    path: 'Eventos', component: EventosComponent,
    children: [
      { path: 'EventoDetalhe/:id', component: EventoDetalheComponent },
      { path: 'EventoDetalhe', component: EventoDetalheComponent },
      { path: 'EventoLista', component: EventoListagemComponent },
    ],
  },
  { path: 'Contatos', component: ContatosComponent },
  { path: 'Dashboard', component: DashboardComponent },
  { path: 'Palestrantes', component: PalestrantesComponent },
  { path: '', redirectTo: 'Dashboard', pathMatch: 'full' },
  { path: '**', redirectTo: 'Dashboard', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
