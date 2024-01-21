using Infrastructure.Extensions;
using JwstFeederHandler.DAL;
using JwstFeederHandler.DAL.EntityModels;
using JwstFeederHandler.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.BL;

internal class LimitedSourcesManager
{
    #region Properties
    public int[] LimitedSourceIds { get; } = {
        (int) eSourceType.Youtube,
        (int) eSourceType.HarvadsAbs,
        (int) eSourceType.WebbTelescopeImagesOrg,
        (int) eSourceType.WebbTelescopeOrg,
        (int) eSourceType.Arxiv
    };
    #endregion

    #region Data Members
    private bool isHourPassed { get; } = DateTime.Now.Minute.IsBetween(1, 11);
    private WebbDbContext dbContext { get; set; }
    #endregion

    #region Public Methods
    public LimitedSourcesManager SetDbContext(WebbDbContext dbContext)
    {
        this.dbContext = dbContext;

        return this;
    }

    public IEnumerable<FeedSource> GetLimitedSources()
    {
        var activeLimitedSources = new List<(bool isShouldGet, Func<EntityFeedSourceModel, bool> filterPredicate)>
        {
            (this.isHourPassed,  s => s.SourceTypeID == (int)eSourceType.HarvadsAbs),
            (this.isHourPassed,  s => s.SourceTypeID == (int)eSourceType.Arxiv),
            (isShouldGetYoutube(), s => s.SourceTypeID == (int)eSourceType.Youtube),
            (isShouldGetWebbtelescope(), s => s.SourceTypeID == (int)eSourceType.WebbTelescopeImagesOrg || s.SourceTypeID == (int)eSourceType.WebbTelescopeOrg)
        }
        .Where(s => s.isShouldGet)
        .Select(s => getSingleSourceWithCondition(s.filterPredicate))
        .SelectMany(s => s);

        return activeLimitedSources;
    }
    #endregion

    #region Private Methods
    private bool isShouldGetYoutube()
    {
        bool isHourEven = DateTime.Now.Hour % 2 == 0;

        return this.isHourPassed && isHourEven;
    }

    private bool isShouldGetWebbtelescope()
    {
        int currentMinute = DateTime.Now.Minute;
        bool isAroundRoundHour = currentMinute.IsBetween(51, 59) || currentMinute.IsBetween(1, 11);

        return isAroundRoundHour;
    }

    private IEnumerable<FeedSource> getSingleSourceWithCondition(Func<EntityFeedSourceModel, bool> sourceCondition)
        =>
        this.dbContext
        .FeedSources
        .Where(sourceCondition)
        .Where(s => s.IsActive)
        .Select(s => new FeedSource()
        {
            SourceType = s.SourceTypeID.ToEnum<eSourceType>(),
            InputType = s.InputTypeID.ToEnum<eInputType>(),
            Url = s.Url
        })
        .ToList();
    #endregion
}
