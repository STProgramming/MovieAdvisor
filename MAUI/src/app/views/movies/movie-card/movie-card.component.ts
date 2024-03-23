import { Component, EventEmitter, Input, Output, input } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';
import { Observable } from 'rxjs';
import { MoviesService } from '../movies-services/movies.service';
import { Dialog } from '@angular/cdk/dialog';
import { MovieDetailsModalComponent } from '../movie-details-modal/movie-details-modal.component';

@Component({
  selector: 'app-movie-card',
  templateUrl: './movie-card.component.html',
  styleUrl: './movie-card.component.scss'
})
export class MovieCardComponent {
  Image: any;
  ImageObservable: Observable<Blob>;
  @Input() movieData: MovieDto;
  
  constructor(private movieService: MoviesService,
    private dialog: Dialog){}

  ngOnInit(): void{
    this.loadImageMovie();
  }

  loadImageMovie(){
    this.loadImage(this.movieData.movieId, 0);
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

  openDialogDetail(movie: MovieDto){
    this.dialog.open(MovieDetailsModalComponent, {
      minWidth: '500px',
      data: {
        'movieId': movie.movieId,
        'movieTitle': movie.movieTitle,
        'movieYearProduction': movie.movieYearProduction,
        'movieDescription': movie.movieDescription,
        'movieMaker': movie.movieMaker,
        'movieLifeSpan': movie.movieLifeSpan,
        'isForAdult': movie.isForAdult,
        'tags': movie.tags,
        'images': movie.images
      },
    });
  }
}
