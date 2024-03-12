import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class RecommendationsService {

  constructor(private apiService: RestApiService) { }
}
