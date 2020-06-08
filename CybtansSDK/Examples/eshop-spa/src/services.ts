import { Product, GetProductRequest, GetProductListRequest, GetProductListResponse } from "./model";

type Fetch = (input: RequestInfo, init?: RequestInit)=> Promise<Response>;
type ErrorInfo = {status:number, statusText:string, text: string };

interface CatalogServiceOptions{
    baseUrl:string;
}

export class BaseCatalogService {
    protected _options:CatalogServiceOptions;
    protected _fetch:Fetch;
    protected _headers =  { 'Content-Type': 'application/json', 'Accept': 'application/json' };

    constructor(fetch:Fetch, options:CatalogServiceOptions){
        this._fetch = fetch;
        this._options = options;
    }

    protected getQueryString(data:any):string|undefined {
        if(!data)
            return '';

        let args = [];
        for (const key in data) {
            if (data.hasOwnProperty(key)) {                
                const element = data[key];
                if(element){
                    if(element instanceof Array){
                        element.forEach(e=>args.push(key+'='+ encodeURIComponent(e)) );
                    }else{
                        args.push(key+'='+ encodeURIComponent(element));
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


export class CustomerService extends BaseCatalogService {        
    constructor(fetch:Fetch, options:CatalogServiceOptions){
        super(fetch, options);        
    }

    getCustomer(request:GetProductRequest):Promise<Product> {
        let options:RequestInit = {
            method: 'GET',
            headers: { ...this._headers, 'Authorization': 'Bearer' }
        };        
        
        let endpoint = this._options.baseUrl+`/api/catalog/${request.id}/options`;    

        return this._fetch(endpoint, options)      
        .then((response:Response) => {                       
            return this.getObject(response);
        });
    }

    getCustomers(request:GetProductListRequest):Promise<GetProductListResponse>{
        let options:RequestInit = {
            method: 'GET',
            headers: this._headers
        };
        
        let endpoint = `${this._options.baseUrl}/api/catalog/${this.getQueryString(request)}`;    

        return this._fetch(endpoint, options)      
        .then((response:Response) =>this.getObject(response));
    }

    getCustomerNoReturn(request:GetProductRequest):Promise<ErrorInfo|void>{
        let options:RequestInit = {
            method: 'GET',
            headers: this._headers             
        };
        
        let endpoint = `${this._options.baseUrl}/api/catalog/88${request.id}`;    

        return this._fetch(endpoint, options)      
        .then((response:Response) => {                       
            return this.ensureSuccess(response);
        });
    }
}