using Microsoft.AspNetCore.Identity;

namespace Howest.Movies.Models;

public class User : IdentityUser<Guid>
{
    public override Guid Id { get; set; } = Guid.NewGuid();

    public ICollection<MovieReview> Reviews { get; set; } = new List<MovieReview>();
}