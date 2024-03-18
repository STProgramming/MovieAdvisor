import { Component, Input, input } from '@angular/core';
import { ReviewDto } from '../../../shared/models/review-dto';

@Component({
  selector: 'app-review-card',
  templateUrl: './review-card.component.html',
  styleUrl: './review-card.component.scss'
})
export class ReviewCardComponent {
  @Input() reviewData: ReviewDto;

  
}
