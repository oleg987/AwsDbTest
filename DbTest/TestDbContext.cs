using Microsoft.EntityFrameworkCore;

namespace DbTest;

public class TestEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
    {
        
    }
    
    public DbSet<TestEntity> TestTable { get; set; }
}