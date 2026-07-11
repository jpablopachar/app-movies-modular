using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Data.Domain;

namespace Movies.Data.Configurations;

internal class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
{

    public void Configure(EntityTypeBuilder<MovieGenre> builder)
    {
        builder.ToTable("movies_genres", "movies");
        builder.HasKey(mg => new { mg.MovieId, mg.GenreId });


        builder.HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(mg => mg.Genre)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.GenreId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(mg => mg.GenreId);
        builder.HasIndex(mg => mg.MovieId);
    }
}
