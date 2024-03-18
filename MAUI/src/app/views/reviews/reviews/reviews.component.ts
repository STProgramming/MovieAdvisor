import { Component } from '@angular/core';
import { Observable } from 'rxjs';
import { ReviewDto } from '../../../shared/models/review-dto';
import { ReviewsService } from '../reviews-services/reviews.service';
import { RouteService } from '../../../shared/services/route.service';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrl: './reviews.component.scss'
})
export class ReviewsComponent {
  ReviewsObservable: Observable<ReviewDto[]>;
  ReviewsData: ReviewDto[] = [];
  movieSearch: string | null = "";

  constructor(private reviewsServices: ReviewsService,
    private routeService: RouteService){}

  ngOnInit(): void{
    this.loadReviewsData();
  }

  onChangeSearchParameter(event: any){
    this.movieSearch = event.target.value;
    this.loadReviewsData();
  }

  loadReviewsData(){
    this.ReviewsObservable = this.reviewsServices.getReviews(this.movieSearch);
    this.ReviewsObservable.subscribe((response: ReviewDto[]) =>{
      this.ReviewsData = response;
    });
  }

  goNewReview(){
    this.routeService.goNewReview();
  }
}
