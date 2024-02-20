import { Injectable } from '@angular/core';
import { RestApiService } from '../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { TagDto } from '../../shared/models/tag-dto';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  constructor(private apiService: RestApiService) { }

  getTagsMovie(): Observable<TagDto[]>{
    return this.apiService.get('Tag')
      .pipe(map((response : any) => {
        if(response){
          return response;
        }        
      }));
  }

  
}
