import { Component, Input } from '@angular/core';
import { ReviewDto } from '../../../shared/models/review-dto';

@Component({
  selector: 'app-review-accordion',
  templateUrl: './review-accordion.component.html',
  styleUrl: './review-accordion.component.scss'
})
export class ReviewAccordionComponent {

  @Input() reviewData: ReviewDto;

  constructor(){}

  ngOnInit():void{}
}
