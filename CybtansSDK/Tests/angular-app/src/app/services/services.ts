import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders, HttpEvent } from '@angular/common/http';
import { 
  GetAllRequest,
  GetAllCustomerResponse,
  GetCustomerRequest,
  CustomerDto,
  UpdateCustomerRequest,
  DeleteCustomerRequest,
  GetAllCustomerEventResponse,
  GetCustomerEventRequest,
  CustomerEventDto,
  UpdateCustomerEventRequest,
  DeleteCustomerEventRequest,
  GetAllOrderResponse,
  GetOrderRequest,
  OrderDto,
  UpdateOrderRequest,
  DeleteOrderRequest,
  GetAllOrderStateResponse,
  GetOrderStateRequest,
  OrderStateDto,
  UpdateOrderStateRequest,
  DeleteOrderStateRequest,
  GetAllSoftDeleteOrderResponse,
  GetSoftDeleteOrderRequest,
  SoftDeleteOrderDto,
  UpdateSoftDeleteOrderRequest,
  DeleteSoftDeleteOrderRequest,
  UploadImageRequest,
  UploadImageResponse,
  UploadStreamByIdRequest,
  UploadStreamResponse,
  DownloadImageRequest,
  DowndloadImageResponse,
  LoginRequest,
  LoginResponse,
 } from './models';

function getQueryString(data:any): string|undefined {
  if(!data) return '';
  let args = [];
  for (let key in data) {
      if (data.hasOwnProperty(key)) {                
          let element = data[key];
          if(element !== undefined && element !== null && element !== ''){
              if(element instanceof Array){
                  element.forEach(e=>args.push(key + '=' + encodeURIComponent(e instanceof Date ? e.toJSON(): e)) );
              }else if(element instanceof Date){
                  args.push(key + '=' + encodeURIComponent(element.toJSON()));
              }else{
                  args.push(key + '=' + encodeURIComponent(element));
              }
          }
      }
  }

  return args.length > 0 ? '?' + args.join('&') : '';
}


@Injectable({
  providedIn: 'root',
})
export class CustomerService {  

    private headers =  new HttpHeaders({
      'Content-Type': 'application/json',
       Accept: 'application/json',
    });

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllCustomerResponse> {
      return this.http.get<GetAllCustomerResponse>(`/api/Customer${ getQueryString(request) }`, {
          headers: this.headers,
      });
    }
    
    get(request: GetCustomerRequest): Observable<CustomerDto> {
      return this.http.get<CustomerDto>(`/api/Customer/${request.id}`, {
          headers: this.headers,
      });
    }
    
    create(request: CustomerDto): Observable<CustomerDto> {
      return this.http.post<CustomerDto>(`/api/Customer`, request, {
          headers: this.headers,
      });
    }
    
    update(request: UpdateCustomerRequest): Observable<CustomerDto> {
      return this.http.put<CustomerDto>(`/api/Customer/${request.id}`, request, {
          headers: this.headers,
      });
    }
    
    delete(request: DeleteCustomerRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/Customer/${request.id}`, {
          headers: this.headers,
      });
    }


}


@Injectable({
  providedIn: 'root',
})
export class CustomerEventService {  

    private headers =  new HttpHeaders({
      'Content-Type': 'application/json',
       Accept: 'application/json',
    });

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllCustomerEventResponse> {
      return this.http.get<GetAllCustomerEventResponse>(`/api/CustomerEvent${ getQueryString(request) }`, {
          headers: this.headers,
      });
    }
    
    get(request: GetCustomerEventRequest): Observable<CustomerEventDto> {
      return this.http.get<CustomerEventDto>(`/api/CustomerEvent/${request.id}`, {
          headers: this.headers,
      });
    }
    
    create(request: CustomerEventDto): Observable<CustomerEventDto> {
      return this.http.post<CustomerEventDto>(`/api/CustomerEvent`, request, {
          headers: this.headers,
      });
    }
    
    update(request: UpdateCustomerEventRequest): Observable<CustomerEventDto> {
      return this.http.put<CustomerEventDto>(`/api/CustomerEvent/${request.id}`, request, {
          headers: this.headers,
      });
    }
    
    delete(request: DeleteCustomerEventRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/CustomerEvent/${request.id}`, {
          headers: this.headers,
      });
    }


}


@Injectable({
  providedIn: 'root',
})
export class OrderService {  

    private headers =  new HttpHeaders({
      'Content-Type': 'application/json',
       Accept: 'application/json',
    });

    constructor(private http: HttpClient) {}
    
    foo(): Observable<{}> {
      return this.http.get<{}>(`/api/Order/foo`, {
          headers: this.headers,
      });
    }
    
    baar(): Observable<{}> {
      return this.http.get<{}>(`/api/Order/baar`, {
          headers: this.headers,
      });
    }
    
    test(): Observable<{}> {
      return this.http.get<{}>(`/api/Order/test`, {
          headers: this.headers,
      });
    }
    
    argument(): Observable<{}> {
      return this.http.get<{}>(`/api/Order/arg`, {
          headers: this.headers,
      });
    }
    
    uploadImage(request: UploadImageRequest): Observable<UploadImageResponse> {
      return this.http.post<UploadImageResponse>(`/api/Order/upload`, request, {
          headers: this.headers,
      });
    }
    
    uploadStreamById(request: UploadStreamByIdRequest): Observable<UploadStreamResponse> {
      return this.http.post<UploadStreamResponse>(`/api/Order/${request.id}/upload`, request, {
          headers: this.headers,
      });
    }
    
    uploadStream(request: Blob): Observable<UploadStreamResponse> {
      return this.http.post<UploadStreamResponse>(`/api/Order/stream`, request, {
          headers: this.headers,
      });
    }
    
    downloadImage(request: DownloadImageRequest): Observable<DowndloadImageResponse> {
      return this.http.get<DowndloadImageResponse>(`/api/Order/download${ getQueryString(request) }`, {
          headers: this.headers,
      });
    }
    
    getAll(request: GetAllRequest): Observable<GetAllOrderResponse> {
      return this.http.get<GetAllOrderResponse>(`/api/Order${ getQueryString(request) }`, {
          headers: this.headers,
      });
    }
    
    get(request: GetOrderRequest): Observable<OrderDto> {
      return this.http.get<OrderDto>(`/api/Order/${request.id}`, {
          headers: this.headers,
      });
    }
    
    create(request: OrderDto): Observable<OrderDto> {
      return this.http.post<OrderDto>(`/api/Order`, request, {
          headers: this.headers,
      });
    }
    
    update(request: UpdateOrderRequest): Observable<OrderDto> {
      return this.http.put<OrderDto>(`/api/Order/${request.id}`, request, {
          headers: this.headers,
      });
    }
    
    delete(request: DeleteOrderRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/Order/${request.id}`, {
          headers: this.headers,
      });
    }


}


@Injectable({
  providedIn: 'root',
})
export class OrderStateService {  

    private headers =  new HttpHeaders({
      'Content-Type': 'application/json',
       Accept: 'application/json',
    });

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllOrderStateResponse> {
      return this.http.get<GetAllOrderStateResponse>(`/api/OrderState${ getQueryString(request) }`, {
          headers: this.headers.set('Authorization', 'Bearer'),
      });
    }
    
    get(request: GetOrderStateRequest): Observable<OrderStateDto> {
      return this.http.get<OrderStateDto>(`/api/OrderState/${request.id}`, {
          headers: this.headers.set('Authorization', 'Bearer'),
      });
    }
    
    create(request: OrderStateDto): Observable<OrderStateDto> {
      return this.http.post<OrderStateDto>(`/api/OrderState`, request, {
          headers: this.headers.set('Authorization', 'Bearer'),
      });
    }
    
    update(request: UpdateOrderStateRequest): Observable<OrderStateDto> {
      return this.http.put<OrderStateDto>(`/api/OrderState/${request.id}`, request, {
          headers: this.headers.set('Authorization', 'Bearer'),
      });
    }
    
    delete(request: DeleteOrderStateRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/OrderState/${request.id}`, {
          headers: this.headers.set('Authorization', 'Bearer'),
      });
    }


}


@Injectable({
  providedIn: 'root',
})
export class SoftDeleteOrderService {  

    private headers =  new HttpHeaders({
      'Content-Type': 'application/json',
       Accept: 'application/json',
    });

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllSoftDeleteOrderResponse> {
      return this.http.get<GetAllSoftDeleteOrderResponse>(`/api/SoftDeleteOrder${ getQueryString(request) }`, {
          headers: this.headers,
      });
    }
    
    get(request: GetSoftDeleteOrderRequest): Observable<SoftDeleteOrderDto> {
      return this.http.get<SoftDeleteOrderDto>(`/api/SoftDeleteOrder/${request.id}`, {
          headers: this.headers,
      });
    }
    
    create(request: SoftDeleteOrderDto): Observable<SoftDeleteOrderDto> {
      return this.http.post<SoftDeleteOrderDto>(`/api/SoftDeleteOrder`, request, {
          headers: this.headers,
      });
    }
    
    update(request: UpdateSoftDeleteOrderRequest): Observable<SoftDeleteOrderDto> {
      return this.http.put<SoftDeleteOrderDto>(`/api/SoftDeleteOrder/${request.id}`, request, {
          headers: this.headers,
      });
    }
    
    delete(request: DeleteSoftDeleteOrderRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/SoftDeleteOrder/${request.id}`, {
          headers: this.headers,
      });
    }


}


@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {  

    private headers =  new HttpHeaders({
      'Content-Type': 'application/json',
       Accept: 'application/json',
    });

    constructor(private http: HttpClient) {}
    
    login(request: LoginRequest): Observable<LoginResponse> {
      return this.http.post<LoginResponse>(`/api/auth/login`, request, {
          headers: this.headers,
      });
    }


}
