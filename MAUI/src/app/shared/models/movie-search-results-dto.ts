import { MovieDto } from "./movie-dto";

export interface MovieSearchResultsDto{
    moviesCount: number;
    movies: MovieDto[];
    resultsForYear: MovieDto[];
    resultsForLifeSpan: MovieDto[];
    resultsForTitle: MovieDto[];
    resultsForMaker: MovieDto[];
    resultsForTag: MovieDto[] ;
    resultsForDescription: MovieDto[];
}