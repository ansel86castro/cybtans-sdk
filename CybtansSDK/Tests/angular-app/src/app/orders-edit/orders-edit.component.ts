import { OrderDto, CustomerDto, OrderStateDto } from './../services/models';
import { OrderService, CustomerService, OrderStateService } from './../services/services';
import { Component, OnInit, Input, Output, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { OrderTypeEnum } from '../services/models';
import { empty, from } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { FormBuilder, FormGroup } from '@angular/forms';
import { toInteger } from '@ng-bootstrap/ng-bootstrap/util/util';

@Component({
  selector: 'app-orders-edit',
  templateUrl: './orders-edit.component.html',
  styleUrls: ['./orders-edit.component.scss']
})
export class OrdersEditComponent implements OnInit ,OnChanges {

  @Input() id?: string|undefined;
  @Output() cancel = new EventEmitter<void>();

  orderTypes = [
    {label: 'Default', value: OrderTypeEnum.default},
    {label: 'Normal', value: OrderTypeEnum.normal},
    {label: 'Shipping', value: OrderTypeEnum.shipping}];

  item: any = {};
  invalidated = true;
  customers?: CustomerDto[] | null = [];
  orderStates?: OrderStateDto[] | null = [];

  constructor(
    private orderService: OrderService,
    private customerService: CustomerService,
    private orderStateService: OrderStateService) { }

  ngOnChanges(changes: SimpleChanges): void {
    console.log(changes);
  }

  ngOnInit(): void {
    if (this.invalidated === true){
      this.fetchData();
    }
  }

  fetchData(): void {
    if (this.id){
        this.orderService.get({id: this.id}).subscribe(item => this.item = item);
    }

    this.customerService.getAll({}).subscribe(items => this.customers = items.items);
    this.orderStateService.getAll({}).subscribe(items => this.orderStates = items.items);
    this.invalidated = false;
  }

  onSubmit(): void {
    const order: Partial<OrderDto> =
    {
      description: this.item.description,
      customerId: this.item.customerId,
      orderStateId: parseInt(this.item.orderStateId),
      orderType : parseInt(this.item.orderType)
    };

    if (this.id){
       this.orderService.update({
          id: this.id,
          value: order
       }).subscribe(result => {
          console.log(result);
          this.cancel.emit();
       });
    }else{
      this.orderService.create({
        value: order
      })
      .subscribe(result => {
          console.log(result);
          this.cancel.emit();
      });
    }
  }

  OnCancel(): void {
    this.cancel?.emit();
  }
}
