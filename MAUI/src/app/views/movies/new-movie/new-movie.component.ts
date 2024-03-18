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
  private moviePosted = new BehaviorSubject<boolean>(false);
  private moviePostedObs: Observable<boolean> = this.moviePosted;
  private newIdMovie: number =0;

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
      this.moviePosted.next(false);
      this.postNewMovie(newMovieForm); 
      this.moviePostedObs.subscribe(posted => {
        if(posted){
          //this.postNewImage();
          this.routeService.goMovies();
          this.toastr.success('il film '+ newMovieForm.get('title').value + ' è stato inserito correttamente', 'Film inserito');    
        }
      });
    }
  }

  postNewMovie(newMovieForm: FormGroup){
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
    this.NewMovieObservable = this.moviesService.postMovie(newMovie);
    this.NewMovieObservable.subscribe((resp: any) => {
      this.newIdMovie = resp;
      this.moviePosted.next(true);      
    });
  }

  /*
  postNewImage(){
    if(this.filesSelected.length > 0 && this.newIdMovie > 0){
      var listFiles = new FormData();
      this.filesSelected.forEach(e => {
        listFiles.append('fileArray', e, e.name);
      });
      this.NewImageObservable = this.moviesService.postMovieImage(listFiles, this.newIdMovie);
      this.NewImageObservable.subscribe(resp =>{ 
        return resp;       
      });
    } 
  }
  */

}
