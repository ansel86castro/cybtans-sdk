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

type Fetch = (input: RequestInfo, init?: RequestInit)=> Promise<Response>;
type ErrorInfo = {status:number, statusText:string, text: string };

interface TestsOptions{
    baseUrl:string;
}

export class BaseTestsService {
    protected _options:TestsOptions;
    protected _fetch:Fetch;
    protected _headers =  { 'Content-Type': 'application/json', 'Accept': 'application/json' };

    constructor(fetch:Fetch, options:TestsOptions){
        this._fetch = fetch;
        this._options = options;
    }

    protected getQueryString(data:any):string|undefined {
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

    protected getObject<T>(response:Response) : Promise<T>{
        let status = response.status;
        if(status >= 200 && status < 300 ){            
            return response.json();
        }     
        return response.text().then((text) => Promise.reject<T>({  status, statusText:response.statusText, text }));        
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
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Customer`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetCustomerRequest) : Promise<CustomerDto> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CustomerDto) : Promise<CustomerDto> {
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Customer`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateCustomerRequest) : Promise<CustomerDto> {
    	let options:RequestInit = { method: 'PUT', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteCustomerRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Customer/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class CustomerEventService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllCustomerEventResponse> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetCustomerEventRequest) : Promise<CustomerEventDto> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:CustomerEventDto) : Promise<CustomerEventDto> {
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateCustomerEventRequest) : Promise<CustomerEventDto> {
    	let options:RequestInit = { method: 'PUT', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteCustomerEventRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/CustomerEvent/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class OrderService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    foo() : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order/foo`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    baar() : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order/baar`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    test() : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order/test`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    argument() : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order/arg`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }
    
    uploadImage(request:UploadImageRequest) : Promise<UploadImageResponse> {
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order/upload`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    uploadStreamById(request:UploadStreamByIdRequest) : Promise<UploadStreamResponse> {
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}/upload`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    uploadStream(request:Blob) : Promise<UploadStreamResponse> {
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order/stream`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    downloadImage(request:DownloadImageRequest) : Promise<DowndloadImageResponse> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order/download`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllOrderResponse> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetOrderRequest) : Promise<OrderDto> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:OrderDto) : Promise<OrderDto> {
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateOrderRequest) : Promise<OrderDto> {
    	let options:RequestInit = { method: 'PUT', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteOrderRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/Order/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class OrderStateService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllOrderStateResponse> {
    	let options:RequestInit = { method: 'GET', headers: { ...this._headers, 'Authorization': 'Bearer'}};
    	let endpoint = this._options.baseUrl+`/api/OrderState`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetOrderStateRequest) : Promise<OrderStateDto> {
    	let options:RequestInit = { method: 'GET', headers: { ...this._headers, 'Authorization': 'Bearer'}};
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:OrderStateDto) : Promise<OrderStateDto> {
    	let options:RequestInit = { method: 'POST', headers: { ...this._headers, 'Authorization': 'Bearer'}};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/OrderState`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateOrderStateRequest) : Promise<OrderStateDto> {
    	let options:RequestInit = { method: 'PUT', headers: { ...this._headers, 'Authorization': 'Bearer'}};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteOrderStateRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: { ...this._headers, 'Authorization': 'Bearer'}};
    	let endpoint = this._options.baseUrl+`/api/OrderState/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class SoftDeleteOrderService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    getAll(request:GetAllRequest) : Promise<GetAllSoftDeleteOrderResponse> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder`+this.getQueryString(request);
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    get(request:GetSoftDeleteOrderRequest) : Promise<SoftDeleteOrderDto> {
    	let options:RequestInit = { method: 'GET', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    create(request:SoftDeleteOrderDto) : Promise<SoftDeleteOrderDto> {
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    update(request:UpdateSoftDeleteOrderRequest) : Promise<SoftDeleteOrderDto> {
    	let options:RequestInit = { method: 'PUT', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }
    
    delete(request:DeleteSoftDeleteOrderRequest) : Promise<ErrorInfo|void> {
    	let options:RequestInit = { method: 'DELETE', headers: this._headers};
    	let endpoint = this._options.baseUrl+`/api/SoftDeleteOrder/${request.id}`;
    	return this._fetch(endpoint, options).then((response:Response) => this.ensureSuccess(response));
    }

}


export class AuthenticationService extends BaseTestsService {  

    constructor(fetch:Fetch, options:TestsOptions){
        super(fetch, options);        
    }
    
    login(request:LoginRequest) : Promise<LoginResponse> {
    	let options:RequestInit = { method: 'POST', headers: this._headers};
    	options.body = JSON.stringify(request);
    	let endpoint = this._options.baseUrl+`/api/auth/login`;
    	return this._fetch(endpoint, options).then((response:Response) => this.getObject(response));
    }

}
