using Microsoft.EntityFrameworkCore;

namespace api_account.Data;

public class L2AccountContext : DbContext
{
    public L2AccountContext(DbContextOptions<L2AccountContext> options)
        : base(options)
    {

    }
    public DbSet<Account>? Account { get; set; }
}

