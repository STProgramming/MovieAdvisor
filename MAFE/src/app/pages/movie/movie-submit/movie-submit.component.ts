import { HttpStatusCode } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IMovieModel } from 'src/app/interfaces/IMovieDTO';
import { IMovieTagView } from 'src/app/interfaces/IMovieTagDTO';
import { MovieService } from 'src/app/services/api-services/movie.service';
import { MovietagService } from 'src/app/services/api-services/movietag.service';

@Component({
  selector: 'app-movie-submit',
  templateUrl: './movie-submit.component.html',
  styleUrls: ['./movie-submit.component.scss']
})

export class MovieSubmitComponent {
  movieForm: FormGroup = null;
  movieTags: IMovieTagView[] = [];
  errorGenericForm: boolean = false;
  successMessage: boolean = false;
  movieTitle: string = '';
  loading: boolean = false;
  file: File;

  constructor(private readonly movieApi: MovieService,
    private readonly movieTagsApi: MovietagService){}

  ngOnInit(): void {
    this.Loading();
    this.InitForm();
    this.GetMovieTags();
  }

  InitForm(){
    this.movieForm = new FormGroup({
      MovieTitle: new FormControl('', Validators.required),
      MovieYearProduction: new FormControl(new Date, Validators.required),
      MovieDescription: new FormControl('', Validators.required),
      MovieMaker: new FormControl('', Validators.required),
      IsForAdult: new FormControl(),
      MovieTagsId: new FormControl([],Validators.required),
    });
  }

  GetMovieTags(){
    this.movieTagsApi.GetAllMovieTags().subscribe(
      (data: any) => {
        if(data){
          this.movieTags = data;
          this.Loaded();
        }
      }
    );
  }

  LoadImage(event:any){
    this.file = event.target.files[0];
  }

  OnSubmit(newMovieForm: FormGroup){ 
    if(newMovieForm.valid){
      let title: string = newMovieForm.get('MovieTitle').value;
      let year = new Date (newMovieForm.get('MovieYearProduction').value);
      let description: string = newMovieForm.get('MovieDescription').value;
      let movieMaker: string = newMovieForm.get('MovieMaker').value;
      let isAdult: boolean = newMovieForm.get('IsForAdult').value === null ? false : true;
      let movieTags: number[] = newMovieForm.get('MovieTagsId').value;      
      let newMovie: IMovieModel = {
        movieTitle: title,
        movieYearProduction: year.getFullYear(),
        movieDescription: description,
        movieMaker: movieMaker,
        isForAdult:  isAdult,      
        movieTagsId: movieTags,
        fileName: this.file.name
      };   
      this.PostMovie(newMovie);
    }   
  }

  private PostMovie(newMovie: IMovieModel){
    this.movieApi.PostNewMovie(newMovie).subscribe(
      (response: HttpStatusCode) => {
        if(response == 201){
          this.successMessage = true;
          this.movieTitle = newMovie.movieTitle;
          this.errorGenericForm = false;
        }
        else{
          this.successMessage = false;
          this.errorGenericForm = true;
        }
      }
    );
  }

  Loading(){
    this.loading = true;
  }

  Loaded(){
    this.loading = false;
  }
}
