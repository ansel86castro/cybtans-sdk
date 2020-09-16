import { Component, OnInit } from '@angular/core';
import { OrderService } from '../services/services';
import { OrderDto } from '../services/models';

@Component({
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit {

  items?: OrderDto[]|null = [];

  constructor(private orderService: OrderService) { }

  ngOnInit(): void {
     this.orderService.getAll({})
     .subscribe(response => {
        this.items = response.items;
     });
  }


}
