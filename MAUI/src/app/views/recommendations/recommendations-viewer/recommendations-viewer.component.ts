import { Component, Input } from '@angular/core';
import { RecommendationDto } from '../../../shared/models/recommendation-dto';

@Component({
  selector: 'app-recommendations-viewer',
  templateUrl: './recommendations-viewer.component.html',
  styleUrl: './recommendations-viewer.component.scss'
})
export class RecommendationsViewerComponent {
  @Input() recommendation: RecommendationDto;

  constructor(){}

  ngOnInit():void{}
}
