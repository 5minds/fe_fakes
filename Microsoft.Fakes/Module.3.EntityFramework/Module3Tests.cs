namespace Module._3.EntityFramework
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Fakes;
    using System.Linq;

    using Microsoft.QualityTools.Testing.Fakes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Module._3.Fakes;

    [TestClass]
    public class Module3Tests
    {
        [TestMethod]
        public void GetMoviesByGenre_OnMixedList_ShouldReturn3()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var movies = new List<Movie>
                                 {
                                     new Movie { IMDBId = "tt2388715", Genre = "Horror", Title = "Oculus", Year = new DateTime(2014, 4, 3) },
                                     new Movie { IMDBId = "tt0063522", Genre = "Horror", Title = "Rosemaries Baby", Year = new DateTime(1968, 10, 17) },
                                     new Movie { IMDBId = "tt0120338", Genre = "Romance", Title = "Titanic", Year = new DateTime(1998, 1, 8) },
                                     new Movie { IMDBId = "tt1457767", Genre = "Horror", Title = "Conjuring - Die Heimsuchung", Year = new DateTime(2013, 8, 1) }
                                 };

                /*
                 * Binding Interfaces
                 * 
                 * When a shimmed type implements an interface, the code generator emits a method that allows it to bind all the members 
                 * from that interface at once.
                 */
                var movieSet = new ShimDbSet<Movie>().Bind(movies.AsQueryable());
                var context = new ShimMovieDbContext { MoviesGet = () => movieSet.Instance };
                var factory = new StubIMovieContextFactory { Create = () => context };
                var repository = new MovieRepository(factory);

                // act
                var horrorMovies = repository.GetMoviesByGenre("Horror");

                // assert
                Assert.AreEqual(horrorMovies.Count(), 3);
            }
        }

        [TestMethod]
        public void GetMoviesByAwesomeness_OnMixedList_ShouldReturn4RatedAnd1Unrated()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var movies = new List<Movie>
                                 {
                                     new Movie { IMDBId = "tt2388715", Genre = "Horror", Title = "Oculus", Year = new DateTime(2014, 4, 3), Rating = null },
                                     new Movie { IMDBId = "tt0063522", Genre = "Horror", Title = "Rosemaries Baby", Year = new DateTime(1968, 10, 17), Rating = 8.0 },
                                     new Movie { IMDBId = "tt0120338", Genre = "Romance", Title = "Titanic", Year = new DateTime(1998, 1, 8), Rating = 7.7 },
                                     new Movie { IMDBId = "tt1457767", Genre = "Horror", Title = "Conjuring - Die Heimsuchung", Year = new DateTime(2013, 8, 1), Rating = 7.6 },
                                     new Movie { IMDBId = "tt0111161", Genre = "Drama", Title = "Die Verurteilten", Year = new DateTime(1995, 3, 9), Rating = 9.3 }
                                 };
                var movieSet = new ShimDbSet<Movie>().Bind(movies.AsQueryable());
                var context = new ShimMovieDbContext { MoviesGet = () => movieSet.Instance };
                var factory = new StubIMovieContextFactory { Create = () => context };
                var repository = new MovieRepository(factory);

                // act
                var ratedMovies = repository.GetMoviesByAwesomeness();

                // assert
                Assert.AreEqual(ratedMovies.Count(), 4);
                Assert.AreEqual(ratedMovies.First().Title, "Die Verurteilten");
                Assert.IsNull(ratedMovies.FirstOrDefault(m => m.IMDBId == "tt2388715"));
            }
        }

        [TestMethod]
        public void MoviesAdd_NewEntity_ShoudlAddAndSave()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var saveCalled = false;
                var addCalled = false;
                
                var movie = new Movie { IMDBId = "tt0089218", Genre = "Adventure", Title = "Die Goonies", Year = new DateTime(1985, 12, 19), Rating = 7.8 };
                var movies = Enumerable.Empty<Movie>().ToList();
                var movieSet = new ShimDbSet<Movie>().Bind(movies.AsQueryable());
                movieSet.AddT0 = (m) =>
                    {
                        movies.Add(m);
                        addCalled = true;
                        return m;
                    };
                var context = new ShimMovieDbContext { MoviesGet = () => movieSet.Instance };
                var baseContext = new ShimDbContext(context) 
                    { 
                        SaveChanges = () =>
                                            {
                                                saveCalled = true;
                                                return 0;
                                            }
                    };
                var factory = new StubIMovieContextFactory { Create = () => context };
                var repository = new MovieRepository(factory);

                // act
                repository.AddMovie(movie);

                // assert
                Assert.AreEqual(movies.Count, 1);
                Assert.AreEqual(movies.First().Title, "Die Goonies");
                Assert.IsTrue(addCalled);
                Assert.IsTrue(saveCalled);
            }
        }

        [TestMethod]
        public void MoviesAdd_ExistingEntity_ShoudlNotAttach()
        {
            using (ShimsContext.Create())
            {
                // arrange
                var saveCalled = false;
                var addCalled = false;

                var movie = new Movie { IMDBId = "tt0089218", Genre = "Adventure", Title = "Die Goonies", Year = new DateTime(1985, 12, 19), Rating = 7.8 };
                var movies = new List<Movie> { new Movie { IMDBId = "tt0089218", Genre = "Adventure", Title = "Die Goonies", Year = new DateTime(1985, 12, 19), Rating = 7.8 } };
                var movieSet = new ShimDbSet<Movie>().Bind(movies.AsQueryable());
                movieSet.AddT0 = (m) =>
                {
                    movies.Add(m);
                    addCalled = true;
                    return m;
                };
                var context = new ShimMovieDbContext { MoviesGet = () => movieSet.Instance };
                var baseContext = new ShimDbContext(context)
                {
                    SaveChanges = () =>
                    {
                        saveCalled = true;
                        return 0;
                    }
                };
                var factory = new StubIMovieContextFactory { Create = () => context };
                var repository = new MovieRepository(factory);

                // act
                repository.AddMovie(movie);

                // assert
                Assert.AreEqual(movies.Count, 1);
                Assert.AreEqual(movies.First().Title, "Die Goonies");
                Assert.IsFalse(addCalled);
                Assert.IsFalse(saveCalled);
            }
        }
    }
}
