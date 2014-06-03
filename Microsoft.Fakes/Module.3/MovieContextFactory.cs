namespace Module._3
{
    public class MovieContextFactory
        : IMovieContextFactory
    {
        public MovieDbContext Create()
        {
            return new MovieDbContext();
        }
    }
}