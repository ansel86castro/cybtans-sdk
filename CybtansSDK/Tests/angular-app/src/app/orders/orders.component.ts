import { Component, OnInit, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { OrderService } from '../services/services';
import { OrderDto } from '../services/models';
import { debounce } from 'debounce';

@Component({
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.scss']
})
export class OrdersComponent implements OnInit , AfterViewChecked {

  items?: OrderDto[]|null = [];
  invalidated = true;
  search?: string;
  edit = false;
  selected?: string;
  @ViewChild('editPanel', {read: ElementRef}) editPanel: ElementRef<HTMLDivElement>;

  constructor(private orderService: OrderService) {
    this.Search = debounce(this.Search.bind(this), 500);
  }

  ngOnInit(): void {
    this.FetchData();
  }

  FetchData(): void {
    if (true === this.invalidated){
      this.orderService.getAll({
        filter: this.search &&
        `description like '%${this.search}%'
        or customer.name like'${this.search}%'
        or orderState.name like '${this.search}%'`,
        take: 50
    })
      .subscribe(response => {
          this.items = response.items;
          this.invalidated = false;
      });
    }
  }

  OnCreateOrder(): void {
    this.edit = true;
  }

  OnEdit(id: string, event: any){
      event.preventDefault();

      this.selected = id;
      this.edit = true;
  }

  ngAfterViewChecked() {
    if (this.edit === true && this.editPanel.nativeElement){
      this.editPanel.nativeElement.scrollIntoView({
        behavior: 'smooth',
        block: 'nearest'
    });
  }
  }

  Search(): void{
    this.invalidated = true;
    this.FetchData();
  }

  onSearchChange(value: string): void{
      this.search = value;
      this.Search();
  }

  onEditComplete(){
     this.invalidated = true;
     this.edit = false;
     this.selected = undefined;

     this.FetchData();
  }

}
