import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from '@angular/common';

import { ContentPanelRoutingModule } from './content-panel-routing.module';
import { HomeComponent } from './home/home.component';
import { SafeHtmlModule } from './../shared/helpers/safe-html.module';

import { MatChipsModule } from '@angular/material/chips';
import { ReadCompositionComponent } from './read-composition/read-composition.component';
import { MatButtonModule } from '@angular/material/button';


@NgModule({
  declarations: [HomeComponent, ReadCompositionComponent],
  imports: [
    CommonModule,
    ContentPanelRoutingModule,
    SafeHtmlModule,
    MatChipsModule,
    MatButtonModule,
    NgbModule
  ]
})
export class ContentPanelModule { }
