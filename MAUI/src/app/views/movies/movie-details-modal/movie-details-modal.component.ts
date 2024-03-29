import { Component, Inject, Input } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';
import { Observable } from 'rxjs';
import { MoviesService } from '../movies-services/movies.service';
import { DIALOG_DATA } from '@angular/cdk/dialog';

@Component({
  selector: 'app-movie-details-modal',
  templateUrl: './movie-details-modal.component.html',
  styleUrl: './movie-details-modal.component.scss'
})
export class MovieDetailsModalComponent {
  isLoading: boolean = false;
  Images: any[] = [];
  ImageObservable: Observable<Blob>;

  constructor(private movieService: MoviesService,
    @Inject(DIALOG_DATA) public data: MovieDto){}

  ngOnInit(): void{
    this.loadImagesMovie();
  }

  loadImagesMovie(){
    this.isLoading = true;
    if (this.data.images.length == 0) this.isLoading = false;
    this.data.images.forEach((image, index) => {
      this.loadImage(this.data.movieId, index);
    });
  }

  loadImage(movieId: number, index: number){
    this.ImageObservable = this.movieService.getMovieImage(movieId, index);
    this.ImageObservable.subscribe({
      next: (resp) => {
        this.convertFromBlobToImg(resp);
      },
      error: (error) =>{
        console.log(error);
        this.isLoading = false;
      },
      complete : () => {
        this.isLoading = false;
      }
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
