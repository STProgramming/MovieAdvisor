import { Component, Input } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';
import { Observable } from 'rxjs';
import { MoviesService } from '../movies-services/movies.service';

@Component({
  selector: 'app-movie-details-modal',
  templateUrl: './movie-details-modal.component.html',
  styleUrl: './movie-details-modal.component.scss'
})
export class MovieDetailsModalComponent {
  Images: any[] = [];
  ImageObservable: Observable<Blob>;
  @Input() movieDetails: MovieDto;

  constructor(private movieService: MoviesService){}

  ngOnInit(): void{
    this.loadImagesMovie();
  }

  loadImagesMovie(){
    this.movieDetails.images.forEach((image, index) => {
      this.loadImage(this.movieDetails.movieId, index);
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
