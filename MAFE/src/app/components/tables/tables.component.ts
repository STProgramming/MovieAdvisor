import { Component, Input } from '@angular/core';
import { IMovieView } from 'src/app/interfaces/IMovieDTO';
import { IMovieTagView } from 'src/app/interfaces/IMovieTagDTO';
import { MovietagService } from 'src/app/services/api-services/movietag.service';

@Component({
  selector: 'app-tables',
  templateUrl: './tables.component.html',
  styleUrls: ['./tables.component.scss']
})
export class TablesComponent {
  @Input() item = '';
  public MovieTags: IMovieTagView[] = [];
  public Movies: IMovieView[] = []
  public loading: boolean = true;
  
  constructor(
    private readonly movieTagApi: MovietagService){}

  async NgOnInit(){
    this.LoadingCompoment();
    switch(this.item){
      case 'movie-tags':
        await this.GetMovieTags();
        break;
      case 'reviews':
        break;
    }
    this.LoadFinished();
  }

  async GetMovieTags(){
    this.movieTagApi.GetAllMovieTags().subscribe(
      (result: any) =>{
        this.MovieTags = result;
      }
    );
  }

  LoadingCompoment(){
    this.loading = true;
  }
  
  LoadFinished(){
    this.loading = false;
  }
}
