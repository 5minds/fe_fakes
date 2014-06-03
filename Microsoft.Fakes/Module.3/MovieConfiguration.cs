namespace Module._3
{
    using System.Data.Entity.ModelConfiguration;

    internal sealed class MovieConfiguration
        : EntityTypeConfiguration<Movie>
    {
        public MovieConfiguration()
        {
            this.HasKey(m => m.Id);
            this.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(255);
            this.Property(m => m.IMDBId)
                .IsRequired()
                .HasMaxLength(16);
            this.Property(m => m.Rating)
                .IsOptional();
            this.Property(m => m.Year)
                .IsOptional();
            this.Property(m => m.Genre)
                .IsOptional();
        }         
    }
}