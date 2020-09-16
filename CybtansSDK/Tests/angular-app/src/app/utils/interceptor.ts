import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
  HttpClient,
} from '@angular/common/http';
import { Observable, throwError, from, of, defer } from 'rxjs';
import { catchError, map, tap, retry, retryWhen } from 'rxjs/operators';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { env } from 'process';
import { JsonPipe } from '@angular/common';
import { LoginResponse } from '../services/models';

function fromPromise<T>() {
  return (o: Observable<Promise<T>>) => new Observable<T>(sub1 =>
      o.subscribe(sub2 => sub2.then(sucess => sub1.next(sucess), err => sub1.error(err))));
}

@Injectable({
  providedIn: 'root',
})
export class AppHttpInterceptor implements HttpInterceptor {
  constructor() {
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return new Observable<HttpEvent<any>>(sub => {
       of(req.clone({ url: environment.apiBaseUrl + req.url}))
      .pipe(
        tap(async request => {
          if (request.headers.has('Authorization')) {
            let token = localStorage.getItem('token');
            if (!token) {
               const response = await this.authenticate();
               token = response.token;
               localStorage.setItem('token', token);
            }

            request = request.clone({
              setHeaders: { authorization: `Bearer ${token}`}
            });
          }

          next.handle(request).subscribe(
            response => sub.next(response),
            error => {
              if (error.status === 401){
                 localStorage.removeItem('token');
              }
              sub.error(error);
            },
            () => sub.complete()
          );
        })
      ).subscribe();
    })
    .pipe(
      retry(2)
    );
  }


  private authenticate(): Promise<LoginResponse> {
   return fetch(`${environment.apiBaseUrl}/api/auth/login`,
    {
      method: 'POST',
      headers: { Accept: 'application/json', 'Content-Type': 'application/json' },
      body: JSON.stringify({ username: 'admin', password: 'admin'})
    })
    .then((response: Response) => {
      const status = response.status;
      if (status >= 200 && status < 300 ){
          return response.json();
      }
      return response.text().then((text) => Promise.reject({  status, statusText: response.statusText, text }));
    });
  }

}
