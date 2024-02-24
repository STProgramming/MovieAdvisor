export interface ReviewDto{
    reviewId: number;
    vote: number;
    descriptionVote: string|null;
    dateTimeVote: Date;
    userEmail: string;
    movieTitle: string;
}