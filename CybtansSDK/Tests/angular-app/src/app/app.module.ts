import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import AppComponent from './app.component';
import Header from './header/header.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { OrdersComponent } from './orders/orders.component';
import { OrderstatesComponent } from './orderstates/orderstates.component';
import { UploadComponent } from './upload/upload.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { AppHttpInterceptor } from './utils/interceptor';
import { OrdersEditComponent } from './orders-edit/orders-edit.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    Header,
    OrdersComponent,
    OrderstatesComponent,
    UploadComponent,
    OrdersEditComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    NgbModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AppHttpInterceptor,
      multi: true,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
