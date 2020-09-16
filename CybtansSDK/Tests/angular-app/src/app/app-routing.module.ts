import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrdersComponent } from './orders/orders.component';
import { OrderstatesComponent } from './orderstates/orderstates.component';
import { UploadComponent } from './upload/upload.component';

const routes: Routes = [
  { path: 'orders', component: OrdersComponent },
  { path: 'orderstates', component: OrderstatesComponent },
  { path: 'upload', component: UploadComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
