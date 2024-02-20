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

  constructor(private moviesService : MoviesService,
    private formBuilder : FormBuilder){}

  ngOnInit(): void{
    this.loadTags();
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

  onSubmit() {
    if (this.newMovieForm.valid) {
      console.log(this.newMovieForm.value);
      // Puoi inviare i dati al backend o fare altre operazioni qui
    } else {
      // Gestisci il caso in cui il form non sia valido
    }
  }

}
