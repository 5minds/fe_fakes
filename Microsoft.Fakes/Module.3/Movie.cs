namespace Module._3
{
    using System;

    public class Movie
    {
        public int Id { get; set; }
        public string IMDBId { get; set; }

        public string Title { get; set; }
        
        public DateTime? Year { get; set; }

        public double? Rating { get; set; }

        public string Genre { get; set; }
    }
}