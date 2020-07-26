import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContentPanelRoutingModule } from './content-panel-routing.module';
import { HomeComponent } from './home/home.component';
import { SafeHtmlModule } from './../shared/helpers/safe-html.module';

import { MatChipsModule } from '@angular/material/chips';


@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    ContentPanelRoutingModule,
    SafeHtmlModule,
    MatChipsModule
  ]
})
export class ContentPanelModule { }
