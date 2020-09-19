import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders, HttpEvent, HttpResponse } from '@angular/common/http';
import { 
  GetAllRequest,
  GetAllReadOnlyEntityResponse,
  GetReadOnlyEntityRequest,
  ReadOnlyEntityDto,
  GetAllCustomerResponse,
  GetCustomerRequest,
  CustomerDto,
  CreateCustomerRequest,
  UpdateCustomerRequest,
  DeleteCustomerRequest,
  GetAllCustomerEventResponse,
  GetCustomerEventRequest,
  CustomerEventDto,
  CreateCustomerEventRequest,
  UpdateCustomerEventRequest,
  DeleteCustomerEventRequest,
  GetAllOrderResponse,
  GetOrderRequest,
  OrderDto,
  CreateOrderRequest,
  UpdateOrderRequest,
  DeleteOrderRequest,
  GetAllOrderStateResponse,
  GetOrderStateRequest,
  OrderStateDto,
  CreateOrderStateRequest,
  UpdateOrderStateRequest,
  DeleteOrderStateRequest,
  GetAllSoftDeleteOrderResponse,
  GetSoftDeleteOrderRequest,
  SoftDeleteOrderDto,
  CreateSoftDeleteOrderRequest,
  UpdateSoftDeleteOrderRequest,
  DeleteSoftDeleteOrderRequest,
  LoginRequest,
  LoginResponse,
  UploadImageRequest,
  UploadImageResponse,
  UploadStreamByIdRequest,
  UploadStreamResponse,
  DownloadImageRequest,
  DowndloadImageResponse,
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


function getFormData(data:any): FormData {
    let form = new FormData();
    if(!data)
        return form;
        
    for (let key in data) {
        if (data.hasOwnProperty(key)) {                
            let value = data[key];
            if(value !== undefined && value !== null && value !== ''){
                if (value instanceof Date){
                    form.append(key, value.toJSON());
                }else if(typeof value === 'number' || typeof value === 'bigint' || typeof value === 'boolean'){
                    form.append(key, value.toString());
                }else if(value instanceof File){
                    form.append(key, value, value.name);
                }else if(value instanceof Blob){
                    form.append(key, value, 'blob');
                }else if(typeof value ==='string'){
                    form.append(key, value);
                }else{
                    throw new Error(`value of ${key} is not supported for multipart/form-data upload`);
                }
            }
        }
    }
    return form;
}


@Injectable({
  providedIn: 'root',
})
export class ReadOnlyEntityService {

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllReadOnlyEntityResponse> {
      return this.http.get<GetAllReadOnlyEntityResponse>(`/api/ReadOnlyEntity${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    get(request: GetReadOnlyEntityRequest): Observable<ReadOnlyEntityDto> {
      return this.http.get<ReadOnlyEntityDto>(`/api/ReadOnlyEntity/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class CustomerService {

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllCustomerResponse> {
      return this.http.get<GetAllCustomerResponse>(`/api/Customer${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    get(request: GetCustomerRequest): Observable<CustomerDto> {
      return this.http.get<CustomerDto>(`/api/Customer/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    create(request: CreateCustomerRequest): Observable<CustomerDto> {
      return this.http.post<CustomerDto>(`/api/Customer`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    update(request: UpdateCustomerRequest): Observable<CustomerDto> {
      return this.http.put<CustomerDto>(`/api/Customer/${request.id}`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    delete(request: DeleteCustomerRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/Customer/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class CustomerEventService {

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllCustomerEventResponse> {
      return this.http.get<GetAllCustomerEventResponse>(`/api/CustomerEvent${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    get(request: GetCustomerEventRequest): Observable<CustomerEventDto> {
      return this.http.get<CustomerEventDto>(`/api/CustomerEvent/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    create(request: CreateCustomerEventRequest): Observable<CustomerEventDto> {
      return this.http.post<CustomerEventDto>(`/api/CustomerEvent`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    update(request: UpdateCustomerEventRequest): Observable<CustomerEventDto> {
      return this.http.put<CustomerEventDto>(`/api/CustomerEvent/${request.id}`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    delete(request: DeleteCustomerEventRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/CustomerEvent/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class OrderService {

    constructor(private http: HttpClient) {}
    
    foo(): Observable<{}> {
      return this.http.get<{}>(`/api/Order/foo`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    baar(): Observable<{}> {
      return this.http.get<{}>(`/api/Order/baar`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    test(): Observable<{}> {
      return this.http.get<{}>(`/api/Order/test`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    argument(): Observable<{}> {
      return this.http.get<{}>(`/api/Order/arg`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    uploadImage(request: UploadImageRequest): Observable<UploadImageResponse> {
      return this.http.post<UploadImageResponse>(`/api/Order/upload`, getFormData(request), {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    uploadStreamById(request: UploadStreamByIdRequest): Observable<UploadStreamResponse> {
      return this.http.post<UploadStreamResponse>(`/api/Order/${request.id}/upload`, getFormData(request), {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    uploadStream(request: Blob): Observable<UploadStreamResponse> {
      return this.http.post<UploadStreamResponse>(`/api/Order/stream`, getFormData({ blob: request }), {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    downloadImage(request: DownloadImageRequest): Observable<HttpResponse<Blob>> {
      return this.http.get(`/api/Order/download${ getQueryString(request) }`, {
          observe: 'response',
          responseType: 'blob',
      });
    }
    
    getAll(request: GetAllRequest): Observable<GetAllOrderResponse> {
      return this.http.get<GetAllOrderResponse>(`/api/Order${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    get(request: GetOrderRequest): Observable<OrderDto> {
      return this.http.get<OrderDto>(`/api/Order/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    create(request: CreateOrderRequest): Observable<OrderDto> {
      return this.http.post<OrderDto>(`/api/Order`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    update(request: UpdateOrderRequest): Observable<OrderDto> {
      return this.http.put<OrderDto>(`/api/Order/${request.id}`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    delete(request: DeleteOrderRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/Order/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class OrderStateService {

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllOrderStateResponse> {
      return this.http.get<GetAllOrderStateResponse>(`/api/OrderState${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    get(request: GetOrderStateRequest): Observable<OrderStateDto> {
      return this.http.get<OrderStateDto>(`/api/OrderState/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    create(request: CreateOrderStateRequest): Observable<OrderStateDto> {
      return this.http.post<OrderStateDto>(`/api/OrderState`, request, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    update(request: UpdateOrderStateRequest): Observable<OrderStateDto> {
      return this.http.put<OrderStateDto>(`/api/OrderState/${request.id}`, request, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    delete(request: DeleteOrderStateRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/OrderState/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class SoftDeleteOrderService {

    constructor(private http: HttpClient) {}
    
    getAll(request: GetAllRequest): Observable<GetAllSoftDeleteOrderResponse> {
      return this.http.get<GetAllSoftDeleteOrderResponse>(`/api/SoftDeleteOrder${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    get(request: GetSoftDeleteOrderRequest): Observable<SoftDeleteOrderDto> {
      return this.http.get<SoftDeleteOrderDto>(`/api/SoftDeleteOrder/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    create(request: CreateSoftDeleteOrderRequest): Observable<SoftDeleteOrderDto> {
      return this.http.post<SoftDeleteOrderDto>(`/api/SoftDeleteOrder`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    update(request: UpdateSoftDeleteOrderRequest): Observable<SoftDeleteOrderDto> {
      return this.http.put<SoftDeleteOrderDto>(`/api/SoftDeleteOrder/${request.id}`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    delete(request: DeleteSoftDeleteOrderRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/SoftDeleteOrder/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {

    constructor(private http: HttpClient) {}
    
    login(request: LoginRequest): Observable<LoginResponse> {
      return this.http.post<LoginResponse>(`/api/auth/login`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }

}
