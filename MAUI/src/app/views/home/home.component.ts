import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../../shared/services/authentication.service';
import { GitHubCommitDto } from '../../shared/models/git-commits.dto';
import { HomeService } from './home-services/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  GitHubObservable: Observable<GitHubCommitDto[]>;
  GitCommits: GitHubCommitDto[] = []; 
  isLoadingGit: boolean = false;

  constructor(private homeService: HomeService){}

  ngOnInit(){
    this.loadGitCommits();
  }

  loadGitCommits(){
    this.isLoadingGit = true;
    this.GitHubObservable = this.homeService.getGitCommits();
    this.GitHubObservable.subscribe({
      next: (resp) => {
        this.GitCommits = resp;
      },
      error: (error) =>{
        this.isLoadingGit = false;
      },
      complete: () => {
        this.isLoadingGit = false; 
      }
    });
  }
}
