import { Component, Input } from '@angular/core';
import { SessionDto } from '../../../shared/models/session-dto';
import { RecommendationsService } from '../recommendations-service/recommendations.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-sessions-viewer',
  templateUrl: './sessions-viewer.component.html',
  styleUrl: './sessions-viewer.component.scss'
})
export class SessionsViewerComponent {
  @Input() sessions: SessionDto[] = [];

  constructor(){}

  ngOnInit(): void{}
}
