
//*****************************************************
// <auto-generated>
//   Generated by the cybtans protocol buffer compiler.  DO NOT EDIT!
//   Powered By Cybtans    
// </auto-generated>
//******************************************************

import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { HttpClient, HttpHeaders, HttpEvent, HttpResponse } from '@angular/common/http';
import { 
  GetAllRequest,
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
  GetAllReadOnlyEntityResponse,
  GetReadOnlyEntityRequest,
  ReadOnlyEntityDto,
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
  MultiPathRequest,
  OrderNotification,
  GetAllNamesResponse,
  GetOrderNameRequest,
  OrderNamesDto,
  CreateOrderNameRequest,
  ClientRequest,
  ClientDto,
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
export class CustomerService {

    constructor(private http: HttpClient) {}
    
    /** Returns a collection of CustomerDto */
    getAll(request: GetAllRequest): Observable<GetAllCustomerResponse> {
      return this.http.get<GetAllCustomerResponse>(`/api/Customer${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    /** Returns one CustomerDto by Id */
    get(request: GetCustomerRequest): Observable<CustomerDto> {
      return this.http.get<CustomerDto>(`/api/Customer/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    /** Creates one CustomerDto */
    create(request: CreateCustomerRequest): Observable<CustomerDto> {
      return this.http.post<CustomerDto>(`/api/Customer`, request, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Updates one CustomerDto by Id */
    update(request: UpdateCustomerRequest): Observable<CustomerDto> {
      return this.http.put<CustomerDto>(`/api/Customer/${request.id}`, request, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Deletes one CustomerDto by Id */
    delete(request: DeleteCustomerRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/Customer/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class CustomerEventService {

    constructor(private http: HttpClient) {}
    
    /** Returns a collection of CustomerEventDto */
    getAll(request: GetAllRequest): Observable<GetAllCustomerEventResponse> {
      return this.http.get<GetAllCustomerEventResponse>(`/api/CustomerEvent${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    /** Returns one CustomerEventDto by Id */
    get(request: GetCustomerEventRequest): Observable<CustomerEventDto> {
      return this.http.get<CustomerEventDto>(`/api/CustomerEvent/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    /** Creates one CustomerEventDto */
    create(request: CreateCustomerEventRequest): Observable<CustomerEventDto> {
      return this.http.post<CustomerEventDto>(`/api/CustomerEvent`, request, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Updates one CustomerEventDto by Id */
    update(request: UpdateCustomerEventRequest): Observable<CustomerEventDto> {
      return this.http.put<CustomerEventDto>(`/api/CustomerEvent/${request.id}`, request, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Deletes one CustomerEventDto by Id */
    delete(request: DeleteCustomerEventRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/CustomerEvent/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }

}


/** Order's Service */
@Injectable({
  providedIn: 'root',
})
export class OrderService {

    constructor(private http: HttpClient) {}
    
    /** Hellow; "Func" */
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
    
    /** Upload an image to the server */
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
      return this.http.post<UploadStreamResponse>(`/api/Order/ByteStream`, getFormData({ blob: request }), {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    downloadImage(request: DownloadImageRequest): Observable<HttpResponse<Blob>> {
      return this.http.get(`/api/Order/download${ getQueryString(request) }`, {
          observe: 'response',
          responseType: 'blob',
      });
    }
    
    getMultiPath(request: MultiPathRequest): Observable<{}> {
      return this.http.get<{}>(`/api/Order/${request.param1}multipath/${request.param2}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    sendNotification(request: OrderNotification): Observable<{}> {
      return this.http.post<{}>(`/api/Order/${request.orderId}/notify/${request.userId}`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    getAllNames(): Observable<GetAllNamesResponse> {
      return this.http.get<GetAllNamesResponse>(`/api/Order/names`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    getOrderName(request: GetOrderNameRequest): Observable<OrderNamesDto> {
      return this.http.get<OrderNamesDto>(`/api/Order/names/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    createOrderName(request: CreateOrderNameRequest): Observable<OrderNamesDto> {
      return this.http.post<OrderNamesDto>(`/api/Order/names`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Returns a collection of OrderDto */
    getAll(request: GetAllRequest): Observable<GetAllOrderResponse> {
      return this.http.get<GetAllOrderResponse>(`/api/Order${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    /** Returns one OrderDto by Id */
    get(request: GetOrderRequest): Observable<OrderDto> {
      return this.http.get<OrderDto>(`/api/Order/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    /** Creates one OrderDto */
    create(request: CreateOrderRequest): Observable<OrderDto> {
      return this.http.post<OrderDto>(`/api/Order`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Updates one OrderDto by Id */
    update(request: UpdateOrderRequest): Observable<OrderDto> {
      return this.http.put<OrderDto>(`/api/Order/${request.id}`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Deletes one OrderDto by Id */
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
    
    /** Returns a collection of OrderStateDto */
    getAll(request: GetAllRequest): Observable<GetAllOrderStateResponse> {
      return this.http.get<GetAllOrderStateResponse>(`/api/OrderState${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    /** Returns one OrderStateDto by Id */
    get(request: GetOrderStateRequest): Observable<OrderStateDto> {
      return this.http.get<OrderStateDto>(`/api/OrderState/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    /** Creates one OrderStateDto */
    create(request: CreateOrderStateRequest): Observable<OrderStateDto> {
      return this.http.post<OrderStateDto>(`/api/OrderState`, request, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Updates one OrderStateDto by Id */
    update(request: UpdateOrderStateRequest): Observable<OrderStateDto> {
      return this.http.put<OrderStateDto>(`/api/OrderState/${request.id}`, request, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Deletes one OrderStateDto by Id */
    delete(request: DeleteOrderStateRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/OrderState/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class ReadOnlyEntityService {

    constructor(private http: HttpClient) {}
    
    /** Returns a collection of ReadOnlyEntityDto */
    getAll(request: GetAllRequest): Observable<GetAllReadOnlyEntityResponse> {
      return this.http.get<GetAllReadOnlyEntityResponse>(`/api/ReadOnlyEntity${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    /** Returns one ReadOnlyEntityDto by Id */
    get(request: GetReadOnlyEntityRequest): Observable<ReadOnlyEntityDto> {
      return this.http.get<ReadOnlyEntityDto>(`/api/ReadOnlyEntity/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class SoftDeleteOrderService {

    constructor(private http: HttpClient) {}
    
    /** Returns a collection of SoftDeleteOrderDto */
    getAll(request: GetAllRequest): Observable<GetAllSoftDeleteOrderResponse> {
      return this.http.get<GetAllSoftDeleteOrderResponse>(`/api/SoftDeleteOrder${ getQueryString(request) }`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    /** Returns one SoftDeleteOrderDto by Id */
    get(request: GetSoftDeleteOrderRequest): Observable<SoftDeleteOrderDto> {
      return this.http.get<SoftDeleteOrderDto>(`/api/SoftDeleteOrder/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }
    
    /** Creates one SoftDeleteOrderDto */
    create(request: CreateSoftDeleteOrderRequest): Observable<SoftDeleteOrderDto> {
      return this.http.post<SoftDeleteOrderDto>(`/api/SoftDeleteOrder`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Updates one SoftDeleteOrderDto by Id */
    update(request: UpdateSoftDeleteOrderRequest): Observable<SoftDeleteOrderDto> {
      return this.http.put<SoftDeleteOrderDto>(`/api/SoftDeleteOrder/${request.id}`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }
    
    /** Deletes one SoftDeleteOrderDto by Id */
    delete(request: DeleteSoftDeleteOrderRequest): Observable<{}> {
      return this.http.delete<{}>(`/api/SoftDeleteOrder/${request.id}`, {
          headers: new HttpHeaders({ Accept: 'application/json' }),
      });
    }

}


/** Jwt Authentication Service */
@Injectable({
  providedIn: 'root',
})
export class AuthenticationService {

    constructor(private http: HttpClient) {}
    
    /** Generates an access token */
    login(request: LoginRequest): Observable<LoginResponse> {
      return this.http.post<LoginResponse>(`/api/auth/login`, request, {
          headers: new HttpHeaders({ Accept: 'application/json', 'Content-Type': 'application/json' }),
      });
    }

}


@Injectable({
  providedIn: 'root',
})
export class ClientService {

    constructor(private http: HttpClient) {}
    
    getClient(request: ClientRequest): Observable<ClientDto> {
      return this.http.get<ClientDto>(`/api/clients/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    getClient2(request: ClientRequest): Observable<ClientDto> {
      return this.http.get<ClientDto>(`/api/clients/client2/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }
    
    getClient3(request: ClientRequest): Observable<ClientDto> {
      return this.http.get<ClientDto>(`/api/clients/client3/${request.id}`, {
          headers: new HttpHeaders({ Authorization: 'Bearer', Accept: 'application/json' }),
      });
    }

}
