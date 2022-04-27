namespace Api.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<Advertisement> Advertisements { get; set; }
#pragma warning disable CS8618
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
#pragma warning restore CS8618
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}