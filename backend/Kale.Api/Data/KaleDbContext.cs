using Kale.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kale.Api.Data;

public class KaleDbContext : DbContext
{
    public KaleDbContext(DbContextOptions<KaleDbContext> options) : base(options) { }

    public DbSet<Ingredient> Ingredients => Set<Ingredient>();
    public DbSet<Recipe> Recipes => Set<Recipe>();
    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
            entity.Property(e => e.CostPerUnit).HasPrecision(10, 2);
            entity.Property(e => e.DefaultQuantity).HasPrecision(10, 2);
            entity.Property(e => e.Calories).HasPrecision(10, 2);
            entity.Property(e => e.ProteinG).HasPrecision(10, 2);
            entity.Property(e => e.CarbsG).HasPrecision(10, 2);
            entity.Property(e => e.FatG).HasPrecision(10, 2);
            entity.Property(e => e.FiberG).HasPrecision(10, 2);
            entity.Property(e => e.VitaminAMcg).HasPrecision(10, 2);
            entity.Property(e => e.VitaminCMg).HasPrecision(10, 2);
            entity.Property(e => e.VitaminDMcg).HasPrecision(10, 2);
            entity.Property(e => e.VitaminKMcg).HasPrecision(10, 2);
            entity.Property(e => e.CalciumMg).HasPrecision(10, 2);
            entity.Property(e => e.IronMg).HasPrecision(10, 2);
            entity.Property(e => e.PotassiumMg).HasPrecision(10, 2);
            entity.Property(e => e.SodiumMg).HasPrecision(10, 2);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.MealType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Instructions).IsRequired();
            entity.Property(e => e.DishTags).HasMaxLength(500);
        });

        modelBuilder.Entity<RecipeIngredient>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).HasPrecision(10, 2);
            entity.Property(e => e.Unit).IsRequired().HasMaxLength(20);
            entity.Property(e => e.FlexibilityType).IsRequired().HasMaxLength(20);
            entity.Property(e => e.MinQuantity).HasPrecision(10, 2);
            entity.Property(e => e.MaxQuantity).HasPrecision(10, 2);

            entity.HasOne(e => e.Recipe)
                .WithMany(r => r.RecipeIngredients)
                .HasForeignKey(e => e.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Ingredient)
                .WithMany(i => i.RecipeIngredients)
                .HasForeignKey(e => e.IngredientId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
