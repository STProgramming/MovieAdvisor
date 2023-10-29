export interface IMovieView{
    MovieId: number,
    MovieTitle: string,
    MovieYearProduction: number,
    MovieDescription: string,
    IsForAdult: boolean,
    MovieImage: any
}

export interface IMovieModel{
    MovieTitle: string, 
    MovieYearProduction: number,
    MovieDescription: string,
    IsForAdult: boolean,
    MovieTagsId: number[],
    PathFile: string
}