import {AuthenticationService} from './services/services';
import { env } from 'process';

type Fetch = (input: RequestInfo, init?: RequestInit)=> Promise<Response>;
type ServiceOptions = { baseUrl:string; };

export interface ServiceConstructor<T>{
    new (fetch:Fetch, options: ServiceOptions) : T;
}

export function getService<T>(srvCtor:ServiceConstructor<T>): T{
    return new srvCtor(fetchIntercep, { baseUrl:process.env.REACT_APP_API_URL });
}

export async function fetchIntercep(input: RequestInfo, options?: RequestInit): Promise<Response> {
    if(options && options.headers && (options.headers as Record<string, string>)['Authorization'] === 'Bearer'){
        let token = localStorage.getItem('token');
        if(!token){
            let srv = new AuthenticationService(fetch.bind(window), { baseUrl: process.env.REACT_APP_API_URL })
            let response = await srv.login({ username: 'admin', password: 'admin'});
            localStorage.setItem('token', response.token);
            token = response.token;
        }
        
        (options.headers as any) ['Authorization'] = `Bearer ${token}`;        
    }
  
    let response = await window.fetch(input, options);
    if(response.status == 401) {
        if(options){
            (options.headers as any) ['Authorization'] = 'Bearer';
        }
        localStorage.removeItem('token');
        response = await fetchIntercep(input, options);
    }

    return response;  
}