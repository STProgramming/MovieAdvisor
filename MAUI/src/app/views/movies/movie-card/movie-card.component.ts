import { Component, Input, input } from '@angular/core';
import { MovieDto } from '../../../shared/models/movie-dto';

@Component({
  selector: 'app-movie-card',
  templateUrl: './movie-card.component.html',
  styleUrl: './movie-card.component.scss'
})
export class MovieCardComponent {
  Images: Blob[] = [];
  @Input() movieData: MovieDto;
  
  constructor(){

  }

  ngOnInit(): void{
    
  }
}
