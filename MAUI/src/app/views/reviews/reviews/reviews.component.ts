import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { ReviewDto } from '../../../shared/models/review-dto';
import { ReviewsService } from '../reviews-services/reviews.service';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrl: './reviews.component.scss'
})
export class ReviewsComponent {
  ReviewsObservable: Observable<ReviewDto[]>;
  ReviewsData: ReviewDto[] = [];
  email: string | null;

  constructor(private reviewsServices: ReviewsService){}

  ngOnInit(): void{
  }

  loadReviewsData(){
    if(this.email != null){
      this.ReviewsObservable = this.reviewsServices.getReviews(this.email);
      this.ReviewsObservable.subscribe((response: ReviewDto[]) =>{
        this.ReviewsData = response;
      });
    }
  }

  inputEmail(event: any){
    this.email = event.target.value;
    this.loadReviewsData();
  }
}
