namespace Module._3
{
    using System.Collections.Generic;
    using System.Linq;

    public sealed class MovieRepository
        : IMovieRepository
    {
        private readonly IMovieContextFactory factory;

        private IMovieContextFactory Factory
        {
            get
            {
                return this.factory;
            }
        }

        public MovieRepository(IMovieContextFactory factory)
        {
            this.factory = factory;
        }

        public IEnumerable<Movie> GetMoviesByGenre(string genre)
        {
            using (var context = this.Factory.Create())
            {
                return context.Movies
                    .Where(m => m.Genre == genre).ToList();
            }
        }

        public IOrderedEnumerable<Movie> GetMoviesByAwesomeness()
        {
            using (var context = this.Factory.Create())
            {
                // server side ordering - no good ;-)
                return context.Movies
                    .Where(m => m.Rating.HasValue && m.Rating.Value > 7.2)
                    .ToList()
                    .OrderByDescending(m => m.Rating)
                    .ThenBy(m => m.Year)
                    .ThenBy(m => m.Title);
            }
        }

        public void AddMovie(Movie movie)
        {
            using (var context = this.Factory.Create())
            {
                var foundMovie = context.Movies.FirstOrDefault(m => m.IMDBId == movie.IMDBId);

                if (null == foundMovie)
                {
                    context.Movies.Add(movie);
                    context.SaveChanges();
                }
            }
        }
    }
}