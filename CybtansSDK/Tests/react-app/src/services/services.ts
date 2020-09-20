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
 } from './models';

export type Fetch = (input: RequestInfo, init?: RequestInit)=> Promise<Response>;
export type ErrorInfo = {status:number, statusText:string, text: string };

export interface TestsOptions{
    baseUrl:string;
}

class BaseTestsService {
    protected _options:TestsOptions;
    protected _fetch:Fetch;    

    constructor(fetch:Fetch, options:TestsOptions){
        this._fetch = fetch;
        this._options = options;
    }

    protected getQueryString(data:any): string|undefined {
        if(!data)
            return '';
        let args = [];
        for (let key in data) {
            if (data.hasOwnProperty(key)) {                
                let element = data[key];
                if(element !== undefined && element !== null && element !== ''){
                    if(element instanceof Array){
                        element.forEach(e=> args.push(key + '=' + encodeURIComponent(e instanceof Date ? e.toJSON(): e)));
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
        let status = response.status;
        if(status >= 200 && status < 300 ){            
            return response.json();
        }     
        return response.text().then((text) => Promise.reject<T>({  status, statusText:response.statusText, text }));        
    }

    protected getBlob(response:Response): Promise<Response>{
        let status = response.status;        

        if(status >= 200 && status < 300 ){             
            return Promise.resolve(response);
        }
        return response.text().then((text) => Promise.reject<Response>({  status, statusText:response.statusText, text }));
    }

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
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Customer`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetCustomerRequest) : Promise<CustomerDto> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CreateCustomerRequest) : Promise<CustomerDto> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Customer`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateCustomerRequest) : Promise<CustomerDto> {
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteCustomerRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class CustomerEventService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllCustomerEventResponse> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetCustomerEventRequest) : Promise<CustomerEventDto> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CreateCustomerEventRequest) : Promise<CustomerEventDto> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateCustomerEventRequest) : Promise<CustomerEventDto> {
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteCustomerEventRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


/** Order's Service */
export class OrderService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    foo() : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Order/foo`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    baar() : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Order/baar`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    test() : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Order/test`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    argument() : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Order/arg`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    /** Upload an image to the server */
    uploadImage(request:UploadImageRequest) : Promise<UploadImageResponse> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json' }};
    	options.body = this.getFormData(request);
    	let endpoint = this._options.baseUrl+`/api/Order/upload`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    uploadStreamById(request:UploadStreamByIdRequest) : Promise<UploadStreamResponse> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json' }};
    	options.body = this.getFormData(request);
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}/upload`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    uploadStream(request:Blob) : Promise<UploadStreamResponse> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json' }};
    	options.body = this.getFormData({blob:request});
    	let endpoint = this._options.baseUrl+`/api/Order/stream`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    downloadImage(request:DownloadImageRequest) : Promise<Response> {
    	let options:RequestInit = { method: 'GET', headers: {  }};
    	let endpoint = this._options.baseUrl+`/api/Order/download`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getBlob(response));
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllOrderResponse> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Order`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetOrderRequest) : Promise<OrderDto> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CreateOrderRequest) : Promise<OrderDto> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateOrderRequest) : Promise<OrderDto> {
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteOrderRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class OrderStateService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllOrderStateResponse> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json', Authorization: 'Bearer' }};
    	let endpoint = this._options.baseUrl+`/api/OrderState`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetOrderStateRequest) : Promise<OrderStateDto> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json', Authorization: 'Bearer' }};
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CreateOrderStateRequest) : Promise<OrderStateDto> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', Authorization: 'Bearer', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/OrderState`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateOrderStateRequest) : Promise<OrderStateDto> {
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', Authorization: 'Bearer', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteOrderStateRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json', Authorization: 'Bearer' }};
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class ReadOnlyEntityService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllReadOnlyEntityResponse> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json', Authorization: 'Bearer' }};
    	let endpoint = this._options.baseUrl+`/api/ReadOnlyEntity`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetReadOnlyEntityRequest) : Promise<ReadOnlyEntityDto> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json', Authorization: 'Bearer' }};
    	let endpoint = this._options.baseUrl+`/api/ReadOnlyEntity/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }

}


export class SoftDeleteOrderService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllSoftDeleteOrderResponse> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetSoftDeleteOrderRequest) : Promise<SoftDeleteOrderDto> {
    	let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CreateSoftDeleteOrderRequest) : Promise<SoftDeleteOrderDto> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateSoftDeleteOrderRequest) : Promise<SoftDeleteOrderDto> {
    	let options:RequestInit = { method: 'PUT', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteSoftDeleteOrderRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: { Accept: 'application/json' }};
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


/** Jwt Authentication Service */
export class AuthenticationService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    /** Generates an access token */
    login(request:LoginRequest) : Promise<LoginResponse> {
    	let options:RequestInit = { method: 'POST', headers: { Accept: 'application/json', 'Content-Type': 'application/json' }};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/auth/login`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }

}
