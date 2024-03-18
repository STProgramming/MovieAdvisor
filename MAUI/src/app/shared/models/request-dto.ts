import { RecommendationDto } from "./recommendation-dto";

export interface RequestDto{
    requestId: number;
    whatClientWant: string;
    howClientFeels: string;
    sentiment: boolean;
    recommendations: RecommendationDto[];
    dateTimeRequest: Date;
}