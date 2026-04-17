using Microsoft.EntityFrameworkCore;
using Ofix.Books;
using Ofix.Brands;
using Ofix.CarListings;
using Ofix.FeatureCategories;
using Ofix.Features;
using Ofix.Models;
using Ofix.SubModels;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;


namespace Ofix.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public class OfixDbContext :
    AbpDbContext<OfixDbContext>,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    public DbSet<Book> Books { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Model> Models { get; set; }
    public DbSet<SubModel> SubModels { get; set; }
    public DbSet<FeatureCategory> FeatureCategories  { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<CarListing> CarListings { get; set; }


    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext 
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext .
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    #endregion

    public OfixDbContext(DbContextOptions<OfixDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureBlobStoring();
        
        builder.Entity<Book>(b =>
        {
            b.ToTable(OfixConsts.DbTablePrefix + "Books",
                OfixConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
        });


        /* Configure Brand entity */


        builder.Entity<Brand>(b =>
        {
            b.ToTable(OfixConsts.DbTablePrefix + "Brands", OfixConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(BrandsConsts.MaxNameLength);

            b.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.LogoBlobName)
                .HasMaxLength(256);

            b.Property(x => x.LogoFileName)
                .HasMaxLength(256);

            b.Property(x => x.Status)
                .IsRequired();
        });

        /* Configure Model entity */


        builder.Entity<Model>(b =>
        {
            b.ToTable(OfixConsts.DbTablePrefix + "Models", OfixConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(ModelConsts.MaxNameLength);

            b.HasOne<Brand>()
             .WithMany()
             .HasForeignKey(x => x.BrandId)
             .OnDelete(DeleteBehavior.Restrict);
        });


        /* Configure SubModel entity */

        builder.Entity<SubModel>(b =>
        {
            b.ToTable(OfixConsts.DbTablePrefix + "SubModels", OfixConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(SubModelConsts.MaxNameLength);

            b.Property(x => x.Slug)
                .HasMaxLength(SubModelConsts.MaxSlugLength);

            b.HasOne<Ofix.Models.Model>()
                .WithMany()
                .HasForeignKey(x => x.ModelId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        /* Configure SubModel entity */

        builder.Entity<CarListing>(b =>
        {
            b.ToTable(OfixConsts.DbTablePrefix + "CarListings", OfixConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(256);

            b.Property(x => x.Description)
                .HasMaxLength(2000);

            b.HasOne<SubModel>()
                .WithMany()
                .HasForeignKey(x => x.SubModelId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        /* Configure FeatureCategory entity */

        builder.Entity<FeatureCategory>(b =>
        {
            b.ToTable(OfixConsts.DbTablePrefix + "FeatureCategories", OfixConsts.DbSchema);
            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Icon).HasMaxLength(256);
            b.Property(x => x.OrderNo).IsRequired();
            b.Property(x => x.Status).IsRequired();
        });


        /* Configure Feature entity */

        builder.Entity<Feature>(b =>
        {
            b.ToTable(OfixConsts.DbTablePrefix + "Features", OfixConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Name).IsRequired().HasMaxLength(128);
            b.Property(x => x.Description).HasMaxLength(256);
            b.Property(x => x.OrderNo).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.HasOne<FeatureCategory>()
             .WithMany()
             .HasForeignKey(x => x.FeatureCategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });



        /* Configure your own tables/entities inside here */

        //builder.Entity<YourEntity>(b =>
        //{
        //    b.ToTable(OfixConsts.DbTablePrefix + "YourEntities", OfixConsts.DbSchema);
        //    b.ConfigureByConvention(); //auto configure for the base class props
        //    //...
        //});
    }
}
