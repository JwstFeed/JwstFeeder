using JwstScheduleProvider.DAL.EntityModels;
using JwstScheduleProvider.Model;
using Microsoft.EntityFrameworkCore;

namespace JwstScheduleProvider.DAL;

internal class ScheduleDbContext : DbContext
{
    #region Properties
    public DbSet<Observation> Observations { get; set; }
    public DbSet<EntityUrlModel> ScheduleUrls { get; set; }
    public DbSet<EntityLogModel> ScheduleProcessingLogs { get; set; }
    #endregion

    #region Methods
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        string connStr = "*";

        builder.UseSqlServer(connStr);
    }
    #endregion
}