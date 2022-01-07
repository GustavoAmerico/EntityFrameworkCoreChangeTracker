using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GAmerico.EntityFrameworkCore.ChangeTracker.UnitTest;

public class Database : DbContextTracker
{
    public DbSet<Person> People { get; set; }

    public Database(DbContextOptions options, IEnumerable<IEntityTracker> observers) : base(options, observers)
    {
    }

    public Database(IEnumerable<IEntityTracker> observers) : base(observers)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("Teste", a => a.EnableNullChecks());
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var peronEntity = modelBuilder.Entity<Person>();

        peronEntity.ToTable("Person")
            .HasKey(a => a.Id)
            ;

        peronEntity.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(255)
            ;
    }
}