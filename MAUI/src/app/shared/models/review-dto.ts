export interface ReviewDto{
    reviewId: number;
    vote: number;
    descriptionVote: string|null;
    dateTimeVote: Date;
    userName: string;
    movieTitle: string;
}