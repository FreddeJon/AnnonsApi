namespace Api.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Advertisement> Advertisements { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}