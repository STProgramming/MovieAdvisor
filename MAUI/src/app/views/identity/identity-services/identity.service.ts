import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment.development';
import { LoginDto } from '../../../shared/models/login-dto';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  constructor(private apiService: RestApiService) { }

  loginWithGoogle(): Observable<any>{
    return this.apiService.get('Identity/Authentication/Google?ReturnUrl='+environment.frontUrl)
      .pipe(map((response : any) => {
        if(response){
          return (response);
        }        
        else{
          return null;
        }
      }));
  }

  loginMovieAdvisor(formLogin: LoginDto): Observable<any>{
    return this.apiService.post('Identity/Authentication/MA', formLogin)
      .pipe(map((response: any) => {
        return response;
      }))
  }
}
