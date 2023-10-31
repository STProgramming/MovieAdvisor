export interface IMovieView{
    movieId: number,
    movieTitle: string,
    movieYearProduction: number,
    movieDescription: string,
    isForAdult: boolean,
    movieImage: any
}

export interface IMovieModel{
    movieTitle: string, 
    movieYearProduction: number,
    movieDescription: string,
    movieMaker: string,
    isForAdult: boolean,
    movieTagsId: number[],
    fileName: string
}