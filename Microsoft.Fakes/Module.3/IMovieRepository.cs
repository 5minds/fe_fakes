namespace Module._3
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IMovieRepository
    {
        IEnumerable<Movie> GetMoviesByGenre(string genre);

        IOrderedEnumerable<Movie> GetMoviesByAwesomeness();

        void AddMovie(Movie movie);
    }
}