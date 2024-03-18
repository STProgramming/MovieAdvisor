export interface RecommendationDto{
    recommendationId: number;
    movieId: number;
    movieTitle: string;
    name: string;
    lastName: string;
    email: string;
    aiScore: number;
    see: boolean;
}