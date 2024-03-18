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

  }

  loadingSessions(){
    this.isLoading = true;
    this.SessionsObservable = this.recommendationService.getSessionByUser();
    this.SessionsObservable.subscribe(resp => {
      this.sessions = resp;
      this.isLoading = false;
    });
  }
}
