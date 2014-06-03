namespace Module._3
{
    using System.Data.Entity;

    public class MovieDbContext
        : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MovieConfiguration());
        }
    }
}