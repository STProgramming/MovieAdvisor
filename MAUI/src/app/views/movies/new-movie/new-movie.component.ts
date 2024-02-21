import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TagDto } from '../../../shared/models/tag-dto';
import { Observable } from 'rxjs';
import { MoviesService } from '../movies-services/movies.service';

@Component({
  selector: 'app-new-movie',
  templateUrl: './new-movie.component.html',
  styleUrl: './new-movie.component.scss'
})
export class NewMovieComponent {
  isLoadingTags: boolean = false;
  TagsObservable: Observable<TagDto[]>;
  MoviesObservable: Observable<any>;
  TagsData: TagDto[];
  newMovieForm: FormGroup;
  errorsList: string[];
  yearList: number[] = [];
  genresSelected: string[] = [];
  filesSelected: File[] = [];

  constructor(private moviesService : MoviesService,
    private formBuilder : FormBuilder){}

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
      genres: [[], Validators.required],
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
    this.filesSelected.push(event.target.value);
  }

  removeImage(event: any){
    var index = this.filesSelected.indexOf(event.target.value);
    this.filesSelected.splice(index, 1);
  }

  onSubmit() {
    if (this.newMovieForm.valid) {
      console.log(this.newMovieForm.value);
      // Puoi inviare i dati al backend o fare altre operazioni qui
    } else {
      // Gestisci il caso in cui il form non sia valido
    }
  }

}
