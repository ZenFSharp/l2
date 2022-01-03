using Microsoft.EntityFrameworkCore;

namespace api_character.Data;

public class L2CharacterContext : DbContext
{
    public L2CharacterContext(DbContextOptions<L2CharacterContext> options)
        : base(options)
    {

    }
    public DbSet<Character>? Character { get; set; }
    public DbSet<Race>? Race { get; set; }
}
