namespace MAModels.Exceptions.AI
{
    public class InsufficientReviewsException : Exception
    {
        public InsufficientReviewsException() { }

        public InsufficientReviewsException(string message) : base(message) { }

        public InsufficientReviewsException(string message, Exception inner) : base(message, inner) { }
    }
}

