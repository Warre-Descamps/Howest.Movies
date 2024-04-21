using Microsoft.AspNetCore.Identity;

namespace Howest.Movies.Models;

public class User : IdentityUser<Guid>
{
    public ICollection<Movie> AddedMovies { get; set; } = new List<Movie>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}