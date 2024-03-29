import { Injectable } from '@angular/core';
import { RestApiService } from '../../../shared/services/rest-api.service';
import { Observable, map } from 'rxjs';
import { GitHubCommitDto } from '../../../shared/models/git-commits.dto';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private apiService : RestApiService) { }
  
  getGitCommits(): Observable<GitHubCommitDto[]>{
    return this.apiService.gitHubListCommits()
      .pipe(map((response : any) => {
        return response;    
      }));
  }
}
