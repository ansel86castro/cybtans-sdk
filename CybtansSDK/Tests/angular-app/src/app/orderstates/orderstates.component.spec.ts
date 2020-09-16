import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OrderstatesComponent } from './orderstates.component';

describe('OrderstatesComponent', () => {
  let component: OrderstatesComponent;
  let fixture: ComponentFixture<OrderstatesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OrderstatesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OrderstatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
