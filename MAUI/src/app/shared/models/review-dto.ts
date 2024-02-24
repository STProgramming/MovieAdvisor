export interface ReviewDto{
    ReviewId: number;
    Vote: number;
    DescriptionVote: string|null;
    DateTimeVote: Date;
    UserEmail: string;
    MovieTitle: string;
}