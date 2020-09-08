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
<<<<<<< HEAD
  DowndloadImageResponse,
=======
>>>>>>> master
  LoginRequest,
  LoginResponse,
 } from './models';

<<<<<<< HEAD
export type Fetch = (input: RequestInfo, init?: RequestInit)=> Promise<Response>;
export type ErrorInfo = {status:number, statusText:string, text: string };

export interface TestsOptions{
    baseUrl:string;
}

class BaseTestsService {
    protected _options:TestsOptions;
    protected _fetch:Fetch;    
=======
type Fetch = (input: RequestInfo, init?: RequestInit)=> Promise<Response>;
type ErrorInfo = {status:number, statusText:string, text: string };

interface TestsOptions{
    baseUrl:string;
}

export class BaseTestsService {
    protected _options:TestsOptions;
    protected _fetch:Fetch;
    protected _headers =  { 'Content-Type': 'application/json', 'Accept': 'application/json' };
>>>>>>> master

    constructor(fetch:Fetch, options:TestsOptions){
        this._fetch = fetch;
        this._options = options;
    }

<<<<<<< HEAD
    protected getQueryString(data:any): string|undefined {
=======
    protected getQueryString(data:any):string|undefined {
>>>>>>> master
        if(!data)
            return '';
        let args = [];
        for (let key in data) {
            if (data.hasOwnProperty(key)) {                
                let element = data[key];
                if(element !== undefined && element !== null && element !== ''){
                    if(element instanceof Array){
                        element.forEach(e=>args.push(key + '=' + encodeURIComponent(e)) );
                    }else{
                        args.push(key + '=' + encodeURIComponent(element));
                    }
                }
            }
        }

       return args.length > 0 ? '?' + args.join('&') : '';    
    }

<<<<<<< HEAD
    protected getFormData(data:any): FormData {
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

    protected getObject<T>(response:Response): Promise<T>{
=======
    protected getObject<T>(response:Response) : Promise<T>{
>>>>>>> master
        let status = response.status;
        if(status >= 200 && status < 300 ){            
            return response.json();
        }     
        return response.text().then((text) => Promise.reject<T>({  status, statusText:response.statusText, text }));        
    }

<<<<<<< HEAD
     protected getBlob(response:Response): Promise<Blob>{
        let status = response.status;
        if(status >= 200 && status < 300 ){             
            return response.blob();
        }     
        return response.text().then((text) => Promise.reject<Blob>({  status, statusText:response.statusText, text }));
    }

=======
>>>>>>> master
    protected ensureSuccess(response:Response): Promise<ErrorInfo|void>{
        let status = response.status;
        if(status < 200 || status >= 300){
            return response.text().then((text) => Promise.reject<ErrorInfo>({  status, statusText:response.statusText, text }));        
        }
        return Promise.resolve();
    }
}


export class CustomerService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllCustomerResponse> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Customer`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetCustomerRequest) : Promise<CustomerDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CustomerDto) : Promise<CustomerDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'POST', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Customer`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateCustomerRequest) : Promise<CustomerDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'PUT', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteCustomerRequest) : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'DELETE', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class CustomerEventService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllCustomerEventResponse> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetCustomerEventRequest) : Promise<CustomerEventDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CustomerEventDto) : Promise<CustomerEventDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'POST', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateCustomerEventRequest) : Promise<CustomerEventDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'PUT', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteCustomerEventRequest) : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'DELETE', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class OrderService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    foo() : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/foo`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    baar() : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/baar`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    test() : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/test`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    argument() : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/arg`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    uploadImage(request:UploadImageRequest) : Promise<UploadImageResponse> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json' }};
    	options.body = this.getFormData(request);
=======
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/upload`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    uploadStreamById(request:UploadStreamByIdRequest) : Promise<UploadStreamResponse> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json' }};
    	options.body = this.getFormData(request);
=======
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}/upload`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    uploadStream(request:Blob) : Promise<UploadStreamResponse> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json' }};
    	options.body = this.getFormData(request);
=======
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/stream`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    downloadImage(request:DownloadImageRequest) : Promise<Blob> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: {  }};
    	let endpoint = this._options.baseUrl+`/api/Order/download`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getBlob(response));
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllOrderResponse> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order/download`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllOrderResponse> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetOrderRequest) : Promise<OrderDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:OrderDto) : Promise<OrderDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'POST', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateOrderRequest) : Promise<OrderDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'PUT', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteOrderRequest) : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'DELETE', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class OrderStateService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllOrderStateResponse> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json', Authorization: 'Bearer' }};
=======
    	let options:RequestInit = { method: 'GET', headers: { ...this._headers, 'Authorization': 'Bearer'}};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/OrderState`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetOrderStateRequest) : Promise<OrderStateDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json', Authorization: 'Bearer' }};
=======
    	let options:RequestInit = { method: 'GET', headers: { ...this._headers, 'Authorization': 'Bearer'}};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:OrderStateDto) : Promise<OrderStateDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', Authorization: 'Bearer', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'POST', headers: { ...this._headers, 'Authorization': 'Bearer'}};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/OrderState`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateOrderStateRequest) : Promise<OrderStateDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', Authorization: 'Bearer', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'PUT', headers: { ...this._headers, 'Authorization': 'Bearer'}};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteOrderStateRequest) : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json', Authorization: 'Bearer' }};
=======
    	let options:RequestInit = { method: 'DELETE', headers: { ...this._headers, 'Authorization': 'Bearer'}};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class SoftDeleteOrderService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllSoftDeleteOrderResponse> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetSoftDeleteOrderRequest) : Promise<SoftDeleteOrderDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'GET', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:SoftDeleteOrderDto) : Promise<SoftDeleteOrderDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'POST', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateSoftDeleteOrderRequest) : Promise<SoftDeleteOrderDto> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'PUT', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteSoftDeleteOrderRequest) : Promise<ErrorInfo|void> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json' }};
=======
    	let options:RequestInit = { method: 'DELETE', headers: this._headers};
>>>>>>> master
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class AuthenticationService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    login(request:LoginRequest) : Promise<LoginResponse> {
<<<<<<< HEAD
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
=======
    	let options:RequestInit = { method: 'POST', headers: this._headers};
>>>>>>> master
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/auth/login`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }

}
