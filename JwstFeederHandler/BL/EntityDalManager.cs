using Infrastructure.Extensions;
using JwstFeederHandler.DAL;
using JwstFeederHandler.DAL.EntityModels;
using JwstFeederHandler.Extensions;
using JwstFeederHandler.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.BL;

internal class EntityDalManager : IDalManager
{
    #region Public Methods
    public IEnumerable<FeedSource> GetFeedSources()
    {
        using var dbContext = new WebbDbContext();

        return dbContext
            .FeedSources
            .Where(s => s.IsActive)
            .Select(s => new FeedSource()
            {
                SourceType = s.SourceTypeID.ToEnum<eSourceType>(),
                InputType = s.InputTypeID.ToEnum<eInputType>(),
                Url = s.Url
            })
            .ToList();
    }

    public HashSet<string> GetExistingKeys(eSourceType sourceType)
    {
        int sourceTypeID = (int)sourceType;
        using var dbContext = new WebbDbContext();
        HashSet<string> extraStsciRawKeys = getExtraStsciRawKeys(sourceType);

        return dbContext
            .FeedItems
            .Where(i => i.SourceTypeID == sourceTypeID)
            .Select(i => i.UniqueID)
            .ToHashSet<string>()
            .AddRange(extraStsciRawKeys);
    }

    public void InsertNewItems(IEnumerable<IFeedItem> items)
    {
        IEnumerable<EntityFeedItemModel> entityFeedItems = items
            .Select(i => new EntityFeedItemModel()
            {
                SourceTypeID = i.SourceType.CastToInt(),
                PlotTypeID = i.PlotType.CastToInt(),
                DatePublished = i.DatePublished,
                ThumbnailUrl = i.ThumbnailUrl,
                ClusterIndex = i.ClusterIndex,
                ShortTitle = i.ShortTitle,
                SourceUrl = i.SourceUrl,
                UniqueID = i.UniqueID,
                PlotUrl = i.PlotUrl,
                IsDirty = false,
            });

        bulkInsert<EntityFeedItemModel>(entityFeedItems);
    }

    public void WriteLog(string log)
    {
        using var dbContext = new WebbDbContext();

        EntityLogModel entityLog = new EntityLogModel()
        {
            Log = log
        };

        dbContext
            .Logs
            .Add(entityLog);

        dbContext.SaveChanges();
    }
    #endregion

    #region Private Methods
    private void bulkInsert<T>(IEnumerable<T> collection) where T : class
    {
        using var dbContext = new WebbDbContext();
        dbContext.ChangeTracker.AutoDetectChangesEnabled = false;

        dbContext.BulkInsert<T>(collection, options =>
        {
            options.BatchSize = this.defaultBatchSize;
        });

        dbContext.SaveChanges();
    }
    
    private HashSet<string> getExtraStsciRawKeys(eSourceType sourceType)
    {
        using var dbContext = new WebbDbContext();

        bool isStsciRaw = sourceType.ToString().ToUpper().Contains("STSCIRAW");
        bool isFilteredOut = sourceType.ToString().ToUpper().Contains("FILTEREDOUT");

        return (isStsciRaw, isFilteredOut) switch
        {
            (true, true) => getAllStsciRawKeysExceptFiltered(dbContext),
            (true, false) => getFilteredStsciRawKeys(dbContext),
            _ => new HashSet<string>()
        };
    }

    private HashSet<string> getAllStsciRawKeysExceptFiltered(WebbDbContext dbContext)
        =>
        dbContext
        .FeedItems
        .Where(i =>
               i.SourceTypeID == (int)eSourceType.StsciRawNirspec
            || i.SourceTypeID == (int)eSourceType.StsciRawNircam
            || i.SourceTypeID == (int)eSourceType.StsciRawNiriss
            || i.SourceTypeID == (int)eSourceType.StsciRawMiri)
        .Select(i => i.UniqueID)
        .ToHashSet<string>();

    private HashSet<string> getFilteredStsciRawKeys(WebbDbContext dbContext)
        =>
        dbContext
        .FeedItems
        .Where(i => i.SourceTypeID == (int)eSourceType.StsciRawFilteredOut)
        .Select(i => i.UniqueID)
        .ToHashSet<string>();
    #endregion
}
