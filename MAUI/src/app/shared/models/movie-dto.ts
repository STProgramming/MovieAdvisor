import { ImageDto } from "./image-dto";
import { TagDto } from "./tag-dto";

export interface MovieDto{
    movieId: number;
    movieTitle: string;
    movieYearProduction: number;
    movieDescription: string;
    movieMaker: string;
    isForAdult: boolean;
    tags: TagDto[];
    images: ImageDto[];
}