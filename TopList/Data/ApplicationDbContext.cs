using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;
using TopList.Entity.Base;
using TopList.Entity.EntityModels;

namespace TopList.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //hepsini ayrı sınıfa taşı

            //modelBuilder.ApplyConfiguration(new CategoryMap());
            //modelBuilder.Entity<TopList.Entity.Base.Entity>(e =>
            //{
            //    e.HasKey(x => x.Id);
            //    e.Property(x => x.EntityId);
            //});
            modelBuilder.Entity<TopList.Entity.Base.Entity>( b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<long>("EntityId");

                b.Property<string>("EntityTypeId")
                    .HasMaxLength(450);

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(450);

                b.Property<string>("Slug")
                    .IsRequired()
                    .HasMaxLength(450);

                b.HasKey("Id");

                b.HasIndex("EntityTypeId");

                b.ToTable("Core_Entity");
            });
            modelBuilder.Entity<TopList.Entity.Base.Entity>(b =>
            {
                b.HasOne(x => x.EntityType)
                    .WithMany()
                    .HasForeignKey(x => x.EntityTypeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<EntityType>( b =>
            {
                b.Property<string>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("AreaName")
                    .HasMaxLength(450);

                b.Property<bool>("IsMenuable");

                b.Property<string>("RoutingAction")
                    .HasMaxLength(450);

                b.Property<string>("RoutingController")
                    .HasMaxLength(450);

                b.HasKey("Id");

                b.ToTable("Core_EntityType");

                b.HasData(
                    new
                    {
                        Id = "Category",
                        AreaName = "Catalog",
                        IsMenuable = true,
                        RoutingAction = "CategoryDetail",
                        RoutingController = "Category"
                    },
              
                    new
                    {
                        Id = "Company",
                        AreaName = "Catalog",
                        IsMenuable = false,
                        RoutingAction = "CompanyDetail",
                        RoutingController = "Company"
                    });
            });
            modelBuilder.Entity<CompanyCategory>( b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<long>("CategoryId");

                b.Property<bool>("IsFeaturedCompany");

                b.Property<long>("CompanyId");

                b.HasKey("Id");

                b.HasIndex("CategoryId");

                b.HasIndex("CompanyId");

                b.ToTable("Catalog_CompanyCategory");
            });
            modelBuilder.Entity<CompanyCategory> (b =>
            {
                b.HasOne(x=>x.Category)
                    .WithMany()
                    .HasForeignKey(x=>x.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x=>x.Company)
                    .WithMany(x=>x.Categories)
                    .HasForeignKey(x=>x.CompanyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Category>( b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("Description");

                b.Property<int>("DisplayOrder");

                b.Property<bool>("IncludeInMenu");

                b.Property<bool>("IsDeleted");

                b.Property<bool>("IsPublished");

                b.Property<string>("MetaDescription");

                b.Property<string>("MetaKeywords")
                    .HasMaxLength(450);

                b.Property<string>("MetaTitle")
                    .HasMaxLength(450);

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(450);

                b.Property<long?>("ParentId");

                b.Property<string>("Slug")
                    .IsRequired()
                    .HasMaxLength(450);

                b.Property<long?>("ThumbnailImageId");

                b.HasKey("Id");

                b.HasIndex("ParentId");

                b.HasIndex("ThumbnailImageId");

                b.ToTable("Catalog_Category");
            });
            modelBuilder.Entity<Category>( b =>
            {
                b.HasOne(x=>x.Parent)
                    .WithMany(x=>x.Children)
                    .HasForeignKey(x=>x.ParentId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x=>x.ThumbnailImage)
                    .WithMany()
                    .HasForeignKey(x=>x.ThumbnailImageId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

         
        }
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ValidateEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            ValidateEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ValidateEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                    .Where(x => (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var item in modifiedEntries)
            {
                if (item.Entity is ValidatableObject validatableObject)
                {
                    var validationResults = validatableObject.Validate();
                    if (validationResults.Any())
                    {
                        throw new Entity.Base.ValidationException(item.Entity.GetType(), validationResults);
                    }
                }
            }
        }
    }
}