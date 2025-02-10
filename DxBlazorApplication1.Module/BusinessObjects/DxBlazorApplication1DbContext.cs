using DevExpress.ExpressApp.EFCore.Updating;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.ExpressApp.Design;
using DevExpress.ExpressApp.EFCore.DesignTime;
using DxBlazorApplication1.Module.Rapoarte;

namespace DxBlazorApplication1.Module.BusinessObjects;

// This code allows our Model Editor to get relevant EF Core metadata at design time.
// For details, please refer to https://supportcenter.devexpress.com/ticket/details/t933891/core-prerequisites-for-design-time-model-editor-with-entity-framework-core-data-model.
public class DxBlazorApplication1ContextInitializer : DbContextTypesInfoInitializerBase {
    protected override DbContext CreateDbContext() {
        var optionsBuilder = new DbContextOptionsBuilder<DxBlazorApplication1EFCoreDbContext>()
            .UseSqlServer(";")//.UseSqlite(";") wrong for a solution with SqLite, see https://isc.devexpress.com/internal/ticket/details/t1240173
            .UseChangeTrackingProxies()
            .UseObjectSpaceLinkProxies();
        return new DxBlazorApplication1EFCoreDbContext(optionsBuilder.Options);
    }
}
//This factory creates DbContext for design-time services. For example, it is required for database migration.
public class DxBlazorApplication1DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DxBlazorApplication1EFCoreDbContext> {
    public DxBlazorApplication1EFCoreDbContext CreateDbContext(string[] args) {
        throw new InvalidOperationException("Make sure that the database connection string and connection provider are correct. After that, uncomment the code below and remove this exception.");
        //var optionsBuilder = new DbContextOptionsBuilder<DxBlazorApplication1EFCoreDbContext>();
        //optionsBuilder.UseSqlServer("Integrated Security=SSPI;Data Source=(localdb)\\mssqllocaldb;Initial Catalog=DxBlazorApplication1");
        //optionsBuilder.UseChangeTrackingProxies();
        //optionsBuilder.UseObjectSpaceLinkProxies();
        //return new DxBlazorApplication1EFCoreDbContext(optionsBuilder.Options);
    }
}
[TypesInfoInitializer(typeof(DxBlazorApplication1ContextInitializer))]
public class DxBlazorApplication1EFCoreDbContext : DbContext {
    public DxBlazorApplication1EFCoreDbContext(DbContextOptions<DxBlazorApplication1EFCoreDbContext> options) : base(options) {
    }
    //public DbSet<ModuleInfo> ModulesInfo { get; set; }
    public DbSet<Intrare> Intrari {  get; set; }
    public DbSet<IntrareDetaliu> IntrariDetalii {  get; set; }
    public DbSet<Iesire> Iesiri { get; set; }
    public DbSet<IesireDetaliu> IesiriDetalii { get; set; }
    public DbSet<Gestiune> Gestiuni { get; set; }
    public DbSet<Produs> Produse { get; set; }
    public DbSet<Partener> Parteneri { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseDeferredDeletion(this);
        modelBuilder.SetOneToManyAssociationDeleteBehavior(DeleteBehavior.SetNull, DeleteBehavior.Cascade);
        modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.ChangingAndChangedNotificationsWithOriginalValues);
        modelBuilder.UsePropertyAccessMode(PropertyAccessMode.PreferFieldDuringConstruction);

        modelBuilder.Entity<ReportDataV2>().HasData(new ReportDataV2[]
            {
                new ReportDataV2
                {
                    ID = Guid.Parse("108d9487-846a-4fd3-9c83-c1cdfb72aa04"),
                    DisplayName = "Situatie",
                    PredefinedReportTypeName = typeof(SituatieReport).ToString(),
                    IsInplaceReport = false,
                    DataTypeName = "",
                    ParametersObjectTypeName = typeof(SituatieRPO).ToString()
                }
            });
    }
}
