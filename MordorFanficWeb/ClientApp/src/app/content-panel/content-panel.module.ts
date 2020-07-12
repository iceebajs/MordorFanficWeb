import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContentPanelRoutingModule } from './content-panel-routing.module';
import { HomeComponent } from './home/home.component';


@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    ContentPanelRoutingModule
  ]
})
export class ContentPanelModule { }
