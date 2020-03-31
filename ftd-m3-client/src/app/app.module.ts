import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FtdM3ClientModule } from 'projects/ftd-m3-client/src/public-api';

import { environment } from '../environments/environment';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FtdM3ClientModule.forRoot({apiUrl: environment.apiUrl})
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
