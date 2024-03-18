import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, retry, throwError } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { AuthenticationService } from './authentication.service';
import { IdentityTokenService } from './identity-token.service';

@Injectable({
  providedIn: 'root'
})
export class RestApiService {
  isAuth: Observable<boolean> = this.authService.authenticated;
  httpOptions: any;

  constructor(private http: HttpClient,
    private authService: AuthenticationService,
    private tokenService: IdentityTokenService) {
      this.isAuth.subscribe(auth => {
        if(auth){
          this.httpOptions = {
            headers : new Headers({
              'Content-Type': 'application/json',
              'Authorization': 'Bearer '+this.tokenService.readIdentityToken()
            }),
          };
        }
        else{
          this.httpOptions = {
            headers: new HttpHeaders({
              'Content-Type': 'application/json',
            }),
          }
        }
      })
    }

  get(route: string): Observable<any>{    
    return this.http
      .get<any>(environment.apiUrl+route, this.httpOptions)
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

  downlaod(route: string): Observable<any>{
    let httpHeaders = new HttpHeaders()
         .set('Accept', "image/jpg,*/*");
    return this.http
      .get<any>(environment.apiUrl+route, { headers: httpHeaders, responseType: 'blob' as 'json'})
      .pipe(retry(1), catchError(this.handleError));
  }

  handleError(error: any){
    alert(JSON.stringify(error));
    return throwError(()=>{
      return error;
    });
  }
}
