using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PAPData.Entities;

public class PAPContext : IdentityDbContext<IdentityUser>
{
    public PAPContext(DbContextOptions<PAPContext> options) : base(options) {}
    
    public DbSet<Adopted> Adoptions { get; set; }
    public DbSet<AppliedForAdoption> Applications { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Pet> Pets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Identity configuration is applied
        
        //determining relationships 
        modelBuilder.Entity<Client>()
            .HasOne(c => c.IdentityUser)
            .WithOne() // determining one-side relationship
            .HasForeignKey<Client>(c => c.IdentityUserId)
            .IsRequired(false);
        
        modelBuilder.Entity<Client>()
                .HasMany(c => c.Adoptions)
                .WithOne(a => a.Client)
                .HasForeignKey(a => a.ClientId).OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Client>()
            .HasMany(c => c.AppliedForAdoptions)
            .WithOne(a => a.Client)
            .HasForeignKey(a => a.ClientId).OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<Pet>()
            .HasMany(p => p.AppliedForAdoptions)
            .WithOne(a => a.Pet)
            .HasForeignKey(a => a.PetId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Pet>()
            .HasMany(p => p.Adoptions)
            .WithOne(a => a.Pet)
            .HasForeignKey(a => a.PetId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Adopted>()
            .HasOne(a => a.Client)
            .WithMany(c => c.Adoptions)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Adopted>()
            .HasOne(a => a.Pet)
            .WithMany(p => p.Adoptions)
            .HasForeignKey(a => a.PetId)
            .OnDelete(DeleteBehavior.NoAction);
        
        modelBuilder.Entity<AppliedForAdoption>()
            .HasOne(a => a.Client)
            .WithMany(c => c.AppliedForAdoptions)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.NoAction);
           

        modelBuilder.Entity<AppliedForAdoption>()
            .HasOne(a => a.Pet)
            .WithMany(p => p.AppliedForAdoptions)
            .HasForeignKey(a => a.PetId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}