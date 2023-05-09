using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NutritionalSupplements.Data;

public partial class AcmeDataContext : DbContext
{
    public AcmeDataContext()
    {
    }

    public AcmeDataContext(DbContextOptions<AcmeDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HealthEffect> HealthEffects { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientNutritionalSupplement> IngredientNutritionalSupplements { get; set; }

    public virtual DbSet<NutritionalSupplement> NutritionalSupplements { get; set; }

    public virtual DbSet<NutritionalSupplementHealthEffect> NutritionalSupplementHealthEffects { get; set; }

    public virtual DbSet<NutritionalSupplementPurpose> NutritionalSupplementPurposes { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductIngredient> ProductIngredients { get; set; }

    public virtual DbSet<Provider> Providers { get; set; }

    public virtual DbSet<Purpose> Purposes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=NutritionalSupplement;Username=postgres;Password=1111");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HealthEffect>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("health_effect_pkey");

            entity.ToTable("health_effect");

            entity.HasIndex(e => e.Category, "health_effect_category_unique").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(30)
                .HasColumnName("category");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnName("description");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ingredient_pkey");

            entity.ToTable("ingredient");

            entity.HasIndex(e => e.Name, "unique_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IngredientSource)
                .HasMaxLength(40)
                .HasColumnName("ingredient_source");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
        });

        modelBuilder.Entity<IngredientNutritionalSupplement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("ingredient_nutritional_supplement_pkey");

            entity.ToTable("ingredient_nutritional_supplement");

            entity.HasIndex(e => new { e.NutritionalSupplementId, e.IngredientId }, "ingredient_nutritional_supplement_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.NutritionalSupplementId).HasColumnName("nutritional_supplement_id");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.IngredientNutritionalSupplements)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("ingredient_nutritional_supplement_ingredient_id_fkey");

            entity.HasOne(d => d.NutritionalSupplement).WithMany(p => p.IngredientNutritionalSupplements)
                .HasForeignKey(d => d.NutritionalSupplementId)
                .HasConstraintName("ingredient_nutritional_supplemen_nutritional_supplement_id_fkey");
        });

        modelBuilder.Entity<NutritionalSupplement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nutritional_supplement_pkey");

            entity.ToTable("nutritional_supplement");

            entity.HasIndex(e => e.Name, "nutritional_supplement_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AcceptableDailyIntake)
                .HasPrecision(20, 10)
                .HasColumnName("acceptable_daily_intake");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.ENumber)
                .HasMaxLength(20)
                .HasDefaultValueSql("'no E-number'::character varying")
                .HasColumnName("e_number");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
        });

        modelBuilder.Entity<NutritionalSupplementHealthEffect>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nutritional_supplement_health_effect_pkey");

            entity.ToTable("nutritional_supplement_health_effect");

            entity.HasIndex(e => new { e.HealthEffectId, e.NutritionalSupplementId }, "nutritional_supplement_health_effect_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HealthEffectId).HasColumnName("health_effect_id");
            entity.Property(e => e.NutritionalSupplementId).HasColumnName("nutritional_supplement_id");

            entity.HasOne(d => d.HealthEffect).WithMany(p => p.NutritionalSupplementHealthEffects)
                .HasForeignKey(d => d.HealthEffectId)
                .HasConstraintName("nutritional_supplement_health_effect_health_effect_id_fkey");

            entity.HasOne(d => d.NutritionalSupplement).WithMany(p => p.NutritionalSupplementHealthEffects)
                .HasForeignKey(d => d.NutritionalSupplementId)
                .HasConstraintName("nutritional_supplement_health_ef_nutritional_supplement_id_fkey");
        });

        modelBuilder.Entity<NutritionalSupplementPurpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("nutritional_supplement_purpose_pkey");

            entity.ToTable("nutritional_supplement_purpose");

            entity.HasIndex(e => new { e.NutritionalSupplementId, e.PurposeId }, "nutritional_supplement_purpose_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NutritionalSupplementId).HasColumnName("nutritional_supplement_id");
            entity.Property(e => e.PurposeId).HasColumnName("purpose_id");

            entity.HasOne(d => d.NutritionalSupplement).WithMany(p => p.NutritionalSupplementPurposes)
                .HasForeignKey(d => d.NutritionalSupplementId)
                .HasConstraintName("nutritional_supplement_purpose_nutritional_supplement_id_fkey");

            entity.HasOne(d => d.Purpose).WithMany(p => p.NutritionalSupplementPurposes)
                .HasForeignKey(d => d.PurposeId)
                .HasConstraintName("nutritional_supplement_purpose_purpose_id_fk");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.HasIndex(e => e.Name, "product_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExpirationDate).HasColumnName("expiration_date");
            entity.Property(e => e.ManufacturingDate).HasColumnName("manufacturing_date");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
            entity.Property(e => e.ProviderId).HasColumnName("provider_id");

            entity.HasOne(d => d.Provider).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProviderId)
                .HasConstraintName("product_provider_id_fkey");
        });

        modelBuilder.Entity<ProductIngredient>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_ingredient_pkey");

            entity.ToTable("product_ingredient");

            entity.HasIndex(e => new { e.IngredientId, e.ProductId }, "product_ingredient_pk").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IngredientId).HasColumnName("ingredient_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Ingredient).WithMany(p => p.ProductIngredients)
                .HasForeignKey(d => d.IngredientId)
                .HasConstraintName("product_ingredient_ingredient_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductIngredients)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("product_ingredient_product_id_fkey");
        });

        modelBuilder.Entity<Provider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("provider_pkey");

            entity.ToTable("provider");

            entity.HasIndex(e => e.Name, "provider_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
            entity.Property(e => e.RegistrationCountry)
                .HasMaxLength(100)
                .HasColumnName("registration_country");
        });

        modelBuilder.Entity<Purpose>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("purpose_pkey");

            entity.ToTable("purpose");

            entity.HasIndex(e => e.Name, "purpose_name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(400)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
