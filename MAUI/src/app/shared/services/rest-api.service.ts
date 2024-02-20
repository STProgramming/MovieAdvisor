import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, retry, throwError } from 'rxjs';
import { environment } from '../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class RestApiService {

  constructor(private http: HttpClient) { }

  httpOptions ={
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };

  get(route: string): Observable<any>{
    return this.http
      .get<any>(environment.apiUrl+route)
      .pipe(retry(1), catchError(this.handleError));
  }

  post(route: string, postBody: any): Observable<any>{
    return this.http
      .post<any>(environment.apiUrl+route, JSON.stringify(postBody), this.httpOptions
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  put(route: string, putBody: any): Observable<any>{
    return this.http
      .put<any>(
        environment.apiUrl+route, JSON.stringify(putBody), this.httpOptions
      )
      .pipe(retry(1), catchError(this.handleError));
  }

  handleError(error: any){
    return throwError(()=>{
      return error;
    });
  }
}
