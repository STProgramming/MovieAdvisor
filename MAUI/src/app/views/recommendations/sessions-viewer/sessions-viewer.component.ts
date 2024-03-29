import { Component } from '@angular/core';
import { SessionDto } from '../../../shared/models/session-dto';
import { RecommendationsService } from '../recommendations-service/recommendations.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-sessions-viewer',
  templateUrl: './sessions-viewer.component.html',
  styleUrl: './sessions-viewer.component.scss'
})
export class SessionsViewerComponent {
  SessionsObservable: Observable<SessionDto[]>;
  sessions: SessionDto[] = [];
  isLoading: boolean = false;

  constructor(private recommendationService: RecommendationsService){}

  ngOnInit(): void{
    this.loadingSessions();
  }

  loadingSessions(){
    this.isLoading = true;
    this.SessionsObservable = this.recommendationService.getSessionByUser();
    this.SessionsObservable.subscribe({
      next : (resp) => {
        this.sessions = resp;
      },
      error : (error) => {
        this.isLoading = false;
      },
      complete : () => {
        this.isLoading = false;
      }
    });
  }
}
