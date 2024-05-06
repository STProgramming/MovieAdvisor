import { Component, Input, input } from '@angular/core';
import { RecommendationsService } from '../recommendations-service/recommendations.service';
import { RecommendationDto } from '../../../shared/models/recommendation-dto';
import { Observable } from 'rxjs';
import { NewRequestDto } from '../../../shared/models/new-request-dto';
import { RouteService } from '../../../shared/services/route.service';
import { SessionDto } from '../../../shared/models/session-dto';

@Component({
  selector: 'app-recommendations',
  templateUrl: './recommendations.component.html',
  styleUrl: './recommendations.component.scss'
})
export class RecommendationsComponent {
  response: boolean = false
  byUser: boolean = false;
  byRequest: boolean = false;
  isLoadingSession: boolean = false;
  isLoading: boolean = false;
  RecommendationsData: RecommendationDto[] = [];
  RecommendationsObservable: Observable<RecommendationDto[]>;
  SessionsObservable: Observable<SessionDto[]>;
  SessionsData: SessionDto[] = [];
  RecommendationsRequestObservable: Observable<RecommendationDto[]>;
  request: NewRequestDto;

  constructor(private recommendationsService: RecommendationsService,
    private routeService: RouteService){}

  ngOnInit(): void{
    this.loadingSessions();
  }

  loadingSessions(){
    this.isLoadingSession = true;
    this.SessionsObservable = this.recommendationsService.getSessionByUser();
    this.SessionsObservable.subscribe({
      next : (resp) => {
        this.SessionsData = resp;        
      },
      error : (error) => {
        this.isLoadingSession = false;
      },
      complete : () => {
        this.isLoadingSession = false;
      }
    });
  }

  loadRecommendationsByUser(){   
    this.isLoading = true; 
    this.RecommendationsObservable = this.recommendationsService.getRecommendationsByUser();
    this.RecommendationsObservable.subscribe({
      next: (resp) =>{
        this.RecommendationsData = resp;
      },
      error: (error) =>{
        this.isLoading= false;
        this.response = true;
      },
      complete: () =>{
        this.isLoading= false;
        this.response = true;
      }
    });
  }

  loadRecommendationsByRequest(){
    this.isLoading = true; 
    this.RecommendationsRequestObservable = this.recommendationsService.getRecommendationsByRequest(this.request);
    this.RecommendationsRequestObservable.subscribe({
      next: (resp) => {
        this.RecommendationsData = resp;
      },
      error: (error) => {
        this.isLoading= false;
        this.response = true;
      },
      complete: () =>{
        this.isLoading = false
        this.response = true;
      }
    });
  }

  onByUser(){
    this.byUser = true;
    this.loadRecommendationsByUser();
  }

  onByRequest(){
    this.byRequest = true;
  }

  goNewReview(){
    this.routeService.goNewReview();
  }

  resetComponent(){
    this.routeService.goRecommendations();
  }

  handleEventRequest(data: NewRequestDto){
    this.request = data;
    this.loadRecommendationsByRequest();
  }
}
