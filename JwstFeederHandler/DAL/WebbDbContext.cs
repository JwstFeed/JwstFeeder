using JwstFeederHandler.DAL.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace JwstFeederHandler.DAL;

internal class WebbDbContext : DbContext
{
    #region Properties
    public DbSet<EntityFeedItemModel> FeedItems { get; set; }
    public DbSet<EntityFeedSourceModel> FeedSources { get; set; }
    public DbSet<EntityLogModel> Logs { get; set; }
    #endregion

    #region Ctor
    public WebbDbContext()
    {
    }
    #endregion

    #region Methods
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        string connStr = "*";

        builder.UseSqlServer(connStr);
    }
    #endregion
}