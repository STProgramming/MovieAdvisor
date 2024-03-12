import { Component, EventEmitter, Input, Output, input } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';
import { Observable } from 'rxjs';
import { MoviesService } from '../movies-services/movies.service';

@Component({
  selector: 'app-movie-card',
  templateUrl: './movie-card.component.html',
  styleUrl: './movie-card.component.scss'
})
export class MovieCardComponent {
  Image: any;
  ImageObservable: Observable<Blob>;
  @Input() movieData: MovieDto;
  seeDetails: boolean = false;
  
  constructor(private movieService: MoviesService){}

  ngOnInit(): void{
    this.loadImageMovie();
  }

  loadImageMovie(){
    this.loadImage(this.movieData.movieId, 0);
  }

  clickSeeDetails(){
    this.seeDetails = !this.seeDetails;
  }

  loadImage(movieId: number, index: number){
    this.ImageObservable = this.movieService.getMovieImage(movieId, index);
    this.ImageObservable.subscribe((response: Blob) => {
      this.convertFromBlobToImg(response);
    });
  }

  convertFromBlobToImg(image: Blob){
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.Image = (reader.result);
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
}
