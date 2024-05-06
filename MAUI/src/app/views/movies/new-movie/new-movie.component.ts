import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TagDto } from '../../../shared/models/tag-dto';
import { BehaviorSubject, Observable } from 'rxjs';
import { MoviesService } from '../movies-services/movies.service';
import { environment } from '../../../../environments/environment.development';
import { MovieDto } from '../../../shared/models/movie-dto';
import { ToastrService } from 'ngx-toastr';
import { NewMovieDto } from '../../../shared/models/new-movie-dto';
import { RouteService } from '../../../shared/services/route.service';

@Component({
  selector: 'app-new-movie',
  templateUrl: './new-movie.component.html',
  styleUrl: './new-movie.component.scss'
})
export class NewMovieComponent {
  isLoadingTags: boolean = false;
  TagsObservable: Observable<TagDto[]>;
  MoviesObservable: Observable<any>;
  NewMovieObservable: Observable<any>;
  NewImageObservable: Observable<any>;
  TagsData: TagDto[];
  newMovieForm: FormGroup;
  yearList: number[] = [];
  genresSelected: string[] = [];
  filesSelected: File[] = [];
  isLoading: boolean = false;

  constructor(private moviesService : MoviesService,
    private formBuilder : FormBuilder,
    private toastr: ToastrService,
    private routeService: RouteService){}

  ngOnInit(): void{
    this.loadTags();
    this.initYearArray();
    this.initForm();
  }

  loadTags(){
    this.isLoadingTags = true;
    this.TagsObservable = this.moviesService.getTagsMovie();
    this.TagsObservable.subscribe((response: TagDto[]) => {
      this.TagsData = response;
      this.isLoadingTags = false;
    });
  }

  initForm(){
    this.newMovieForm = this.formBuilder.group({
      title: ['', Validators.required],
      maker: ['', [Validators.required]],
      description: ['', [Validators.required, Validators.minLength(6)]],
      adult:[false, Validators.required],
      lifeSpan: ['', Validators.required],
      year: ['', Validators.required]
    });
  }

  initYearArray(){
    let date = new Date();
    for(let year = date.getFullYear(); year >= 1900; year--){
      this.yearList.push(year);
    }
  }

  selectGenreMovie(event: any){
    let select = (event.target as HTMLInputElement).value;
    let found = this.TagsData.find((obj) => {
      return obj.tagId === parseInt(select);
    });
    let index = this.genresSelected.indexOf(found.tagName);
    if (index !== -1){
      this.genresSelected.splice(index, 1);
    }
    else{      
      this.genresSelected.push(found.tagName);
    }
  }

  selectImageMovie(event: any){
    var uploadImage = event.target.files[0];
    if(this.filesSelected.length > 0){
      this.filesSelected.find(obj => {
        if(obj.name !== uploadImage.name){
          this.filesSelected.push(event.target.files[0]);
        }
      });
    }
    else{
      this.filesSelected.push(event.target.files[0]);
    }
  }

  removeImage(event: any){
    var index = this.filesSelected.indexOf(event.target.value);
    this.filesSelected.splice(index, 1);
  }

  onSubmit(newMovieForm: FormGroup) {
    if (newMovieForm.valid) {
      this.isLoading = true;
      var data = this.prepareMovieData(newMovieForm); 
      this.postNewMovie(data);
    }
  }

  prepareMovieData(newMovieForm: FormGroup): NewMovieDto{
    var tagsList: number[] = [];
    this.genresSelected.forEach(genre => {
      var obj = this.TagsData.find(tag => {
        return tag.tagName === genre;
      });
      tagsList.push(obj.tagId);
    });
    var newMovie: NewMovieDto = { 
      'movieTitle' : newMovieForm.get('title').value,
      'movieYearProduction' : newMovieForm.get('year').value,
      'movieMaker' : newMovieForm.get('maker').value,
      'movieDescription' : newMovieForm.get('description').value,
      'isForAdult' : newMovieForm.get('adult').value,
      'movieLifeSpan': parseInt(newMovieForm.get('lifeSpan').value),
      'tagsId': tagsList,
    };    
    return newMovie;  
  }

  postNewMovie(newMovie: NewMovieDto){
    this.NewMovieObservable = this.moviesService.postMovie(newMovie);
    this.NewMovieObservable.subscribe({
      next: (resp) =>{
        if(this.filesSelected.length > 0){
          this.toastr.info('Caricamento al 50%', 'Film caricato');
          this.postNewImage(parseInt(resp), newMovie.movieTitle);
        }
        else{
          this.toastr.success('E\' stato caricato correttamente il film '+newMovie.movieTitle, 'Film inserito con successo');
          this.routeService.goMovies();
        }
      },
      error: (error)=>{
        this.toastr.error('Il film non è stato caricato correttamente. Riprovare più tardi', 'Ops qualcosa è andato storto');
        console.log(JSON.stringify(error));
      },
      complete: () =>{
        this.isLoading =false;
      }
    });
  }
  
  postNewImage(newIdMovie: number, nameMovie: string){
    if(this.filesSelected.length > 0 && newIdMovie > 0){
      var listFiles = new FormData();
      this.filesSelected.forEach(e => {
        listFiles.append('files', e, e.name);
      });
      this.NewImageObservable = this.moviesService.postMovieImage(listFiles, newIdMovie);
      this.NewImageObservable.subscribe({
        next: (resp)=>{
          this.toastr.success('E\' stato caricato correttamente il film '+nameMovie, 'Film inserito con successo');
          this.routeService.goMovies();
        },
        error: (error) => {
          console.log(JSON.stringify(error));
          this.toastr.error('Il film è stato caricato ma l\'immagine no...', 'Ops qualcosa è andato storto');
        }
      });
    } 
  }
  

}
