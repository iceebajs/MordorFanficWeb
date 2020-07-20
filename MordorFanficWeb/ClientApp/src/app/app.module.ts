import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { AccountPanelModule } from './account-panel/account-panel.module';
import { ContentPanelModule } from './content-panel/content-panel.module';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoginComponent } from './login/login.component';
import { RegistrationComponent } from './registration/registration.component';

import { ConfigService } from './shared/common/config.service';
import { HttpErrorHandler } from './shared/helpers/http-error-hander.service';
import { MessageService } from './shared/helpers/message.service';
import { AuthorizationService } from './shared/services/authorization.service';
import { AuthInterceptor } from './shared/helpers/auth-interceptor';
import { EditorModule } from '@progress/kendo-angular-editor';


@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
    NotFoundComponent,
    LoginComponent,
    RegistrationComponent
  ],
  imports: [
    BrowserModule,
    AccountPanelModule,
    ContentPanelModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    NgbModule,
    HttpClientModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    ReactiveFormsModule,
    MatIconModule,
    EditorModule    
  ],
  providers: [
    ConfigService,
    HttpErrorHandler,
    MessageService,
    AuthorizationService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
