import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MovieDto } from '../../../shared/models/movie-dto';
import { Observable } from 'rxjs';
import { MoviesService } from '../../movies/movies-services/movies.service';
import { NewReviewDto } from '../../../shared/models/new-review-dto';
import { ToastrService } from 'ngx-toastr';
import { RouteService } from '../../../shared/services/route.service';
import { ReviewsService } from '../reviews-services/reviews.service';

@Component({
  selector: 'app-new-review',
  templateUrl: './new-review.component.html',
  styleUrl: './new-review.component.scss'
})
export class NewReviewComponent {
  newReviewForm: FormGroup;
  moviesData: MovieDto[] = [];
  NewReviewObservable: Observable<any>;
  MovieDataObservable: Observable<MovieDto[]>;
  isLoading: boolean = false;

  constructor(
    private movieService: MoviesService,
    private reviewService: ReviewsService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private routeService: RouteService
  ){}

  ngOnInit():void{
    this.initializeForm();
  }

  initializeForm(){
    this.newReviewForm = this.formBuilder.group({
      id: [0, Validators.required],
      description: ['', Validators.minLength(6)],
      vote:[0, Validators.required],
      when: ['', Validators.required],
    });
  }

  loadingMovieData(){
    this.MovieDataObservable = this.movieService.getMovies("");
    this.MovieDataObservable.subscribe((resp : any)=>{
      if(resp){
        this.moviesData = resp;
      }
    })
  }

  onSubmit(reviewForm: FormGroup){
    if(reviewForm.valid){
      this.isLoading = true;
      var newReviewDto : NewReviewDto = {
        'movieId': parseInt(reviewForm.get('id').value),
        'descriptionVote': reviewForm.get('description').value,
        'vote': parseInt(reviewForm.get('vote').value),
        'when': new Date(reviewForm.get('when').value)
      };
      this.NewReviewObservable = this.reviewService.postReview(newReviewDto);
      this.NewReviewObservable.subscribe((resp :any) => {
        if(resp){
          this.toastr.success('Hai recensito il film', 'Recensione inviata c');
          this.isLoading = false;
          this.routeService.goReviews();
        }
      },
      error => {
        this.toastr.error('Qualcosa è andata storto', 'Invio fallito');
        this.isLoading = false;
      }      
      );
    }
  }
}
