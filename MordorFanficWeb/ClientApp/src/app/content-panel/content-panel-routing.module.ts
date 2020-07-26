import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ReadCompositionComponent } from './read-composition/read-composition.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'read/:id', component: ReadCompositionComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ContentPanelRoutingModule { }
