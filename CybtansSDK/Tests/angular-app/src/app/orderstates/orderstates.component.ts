import { OrderStateDto } from './../services/models';
import { Component, OnInit } from '@angular/core';
import { OrderStateService } from '../services/services';

@Component({
  selector: 'app-orderstates',
  templateUrl: './orderstates.component.html',
  styleUrls: ['./orderstates.component.scss']
})
export class OrderstatesComponent implements OnInit {

  items?: OrderStateDto[]|null = [];


  constructor(private orderStatesService: OrderStateService) { }

  ngOnInit(): void {
    this.orderStatesService.getAll({})
    .subscribe(response => {
       this.items = response.items;
    });
  }

}
