export interface NewMovieDto{
    movieTitle: string;
    movieYearProduction: number;
    movieDescription: string;
    movieMaker: string;
    movieLifeSpan: number;
    isForAdult: boolean;
    tagsId: number[];
}