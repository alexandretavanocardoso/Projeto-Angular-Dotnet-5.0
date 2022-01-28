import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContatosComponent } from './components/Contatos/Contatos.component';
import { DashboardComponent } from './components/Dashboard/Dashboard.component';
import { EventoDetalheComponent } from './components/Eventos/EventoDetalhe/EventoDetalhe.component';
import { EventoListagemComponent } from './components/Eventos/EventoListagem/EventoListagem.component';
import { EventosComponent } from './components/Eventos/Eventos.component';
import { PalestrantesComponent } from './components/Palestrantes/Palestrantes.component';
import { PerfilComponent } from './components/Perfil/Perfil.component';

const routes: Routes = [
  {path: 'Eventos', redirectTo: 'Eventos/EventoLista'},
  {
    path: 'Eventos', component: EventosComponent,
    children: [
      { path: 'EventoDetalhe/:id', component: EventoDetalheComponent },
      { path: 'EventoDetalhe', component: EventoDetalheComponent },
      { path: 'EventoLista', component: EventoListagemComponent },
    ],
  },
  { path: 'Perfil', component: PerfilComponent },
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
