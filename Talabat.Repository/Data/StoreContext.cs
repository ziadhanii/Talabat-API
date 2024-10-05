namespace Talabat.Repository.Data;

public class StoreContext(DbContextOptions<StoreContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<ProductBrand> Brands { get; set; }
    public DbSet<ProductCategory> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
}