import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule} from '@angular/common/http';
import { ValueComponent } from './value/value.component';
import { NavComponent } from './nav/nav.component';
import {FormsModule} from '@angular/forms';
import { AuthService } from './_services/auth.service';

@NgModule({
   declarations: [
      AppComponent,
      ValueComponent,
      NavComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,

      // to make use of angular forms
      FormsModule
   ],
   providers: [
      AuthService 
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
