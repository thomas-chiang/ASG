using ASG.Infrastructure.Common.AisaFlowDbSchemas;
using ASG.Infrastructure.Gaia1001Forms.Views;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Common.SqlServerDbContexts;

public class AsiaFlowDbContext : DbContext
{
    public DbSet<PtSyncForm> PtSyncForms { get; set; } = null!;

    public DbSet<PtSyncFormArchive2024> PtSyncFormsArchive2024 { get; set; } = null!;

    public DbSet<PtSyncFormArchive2025> PtSyncFormsArchive2025 { get; set; } = null!;

    public DbSet<Gaia1001Attendance> Gaia1001Attendances { get; set; }

    public AsiaFlowDbContext(DbContextOptions<AsiaFlowDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                                   + "Authentication=Active Directory Interactive; Encrypt=True; Database=AsiaFlowDB";

            optionsBuilder.UseSqlServer(connectionString, sqlOptions => sqlOptions.CommandTimeout(600));
        }
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // for subcutaneous test to not create Gaia1001Attendance table
        modelBuilder.Entity<Gaia1001Attendance>().ToView("Gaia1001AttendanceView");
        
    }
}