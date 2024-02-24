import { Component, Input, input } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';
import { Observable } from 'rxjs';
import { MoviesService } from '../movies-services/movies.service';

@Component({
  selector: 'app-movie-card',
  templateUrl: './movie-card.component.html',
  styleUrl: './movie-card.component.scss'
})
export class MovieCardComponent {
  Images: any[] = [];
  ImageObservable: Observable<Blob>;
  @Input() movieData: MovieDto;
  
  constructor(private movieService: MoviesService){}

  ngOnInit(): void{
    this.loadImagesMovie();
  }

  loadImagesMovie(){
    this.movieData.images.forEach((image, index) => {
      this.loadImage(this.movieData.movieId, index);
    });
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
      this.Images.push(reader.result);
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
}
