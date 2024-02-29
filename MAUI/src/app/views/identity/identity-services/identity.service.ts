import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { environment } from '../../../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class IdentityService {

  constructor(private apiService: RestApiService) { }

  loginWithGoogle(): Observable<any>{
    return this.apiService.get('Identity/Authentication/Google')
      .pipe(map((response : any) => {
        if(response){
          return (response);
        }        
        else{
          return null;
        }
      }));
  }
}
