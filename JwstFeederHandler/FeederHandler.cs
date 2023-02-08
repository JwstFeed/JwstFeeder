using Infrastructure.Extensions;
using Infrastructure.Logs;
using Infrastructure.Model;
using JwstFeederHandler.BL;
using JwstFeederHandler.InputTypes;
using JwstFeederHandler.InputTypes.Model;
using JwstFeederHandler.Mapping;
using JwstFeederHandler.Mapping.Model;
using JwstFeederHandler.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler;

public class FeederHandler : IRunnable
{
    #region Data Members
    private IDalManager dalManager { get; }
    private string processName { get; }
    #endregion

    #region Ctor
    public FeederHandler()
    {
        this.dalManager = DataAccessFactory.GetDalManagerObj();
        LogManager.AddLogger(logger: this.dalManager);
        this.processName = "FeederHandler";
    }
    #endregion

    #region Public Methods
    public void Exec()
        =>
        ((Action)(() =>
        {
            IEnumerable<FeedSource> feedSources = getFeedSources();
            processFeedSources(feedSources);            
            LogManager.CleanLoggers();
        }))
        .Catch((e) =>
        {
            return handleRetrievingSourcesException(e);
        });
    #endregion

    #region Private Methods
    private void processFeedSources(IEnumerable<FeedSource> feedSources)
        =>
        Parallel
        .ForEach(feedSources, f => processSingleFeedSource(f));

    private void processSingleFeedSource(FeedSource feedSource)
        =>
        ((Action)(() =>
        {
            IEnumerable<IFeedItem> newFeedItems = getNewFeedItems(feedSource);
            insertNewItems(newFeedItems);
        }))
        .Catch((e) =>
        {
            return handleFeedProcessException(e, feedSource.SourceType);
        });

    private IEnumerable<IFeedItem> getNewFeedItems(FeedSource feedSource)
    {
        IEnumerable<IFeedItem> currentFeedItems = getCurrentFeedItems(feedSource);
        HashSet<string> existingKeys = getExistingKeys(feedSource.SourceType);

        return currentFeedItems
            .Where(i => !existingKeys.Contains(i.UniqueID));
    }

    private IEnumerable<IFeedItem> getCurrentFeedItems(FeedSource feedSource)
    {
        Stream sourceStream = getStream(feedSource);

        return getFeedItems(feedSource, sourceStream);
    }

    private string handleFeedProcessException(Exception ex, eSourceType feedSourceType)
    {
        string feedName = feedSourceType.ToAbsString();

        return $"{this.processName} | {feedName} | {ex.Message} | {ex.StackTrace}";
    }

    private string handleRetrievingSourcesException(Exception ex)
        =>
        $"{this.processName} | Retrieving Sources | {ex.Message} | {ex.StackTrace}";

    private IEnumerable<FeedSource> getFeedSources()
        =>
        this.dalManager
        .GetFeedSources();

    private HashSet<string> getExistingKeys(eSourceType sourceType)
        =>
        this.dalManager
        .GetExistingKeys(sourceType);

    private void insertNewItems(IEnumerable<IFeedItem> items)
        =>
        this.dalManager
        .InsertNewItems(items);

    private void writeLog(string log)
        =>
        this.dalManager
        .WriteLog(log);

    private IEnumerable<IFeedItem> getFeedItems(IMappable mappableSource, Stream stream)
        =>
        new MapManager()
        .SetMappable(mappableSource)
        .SetStream(stream)
        .GetFeedItems();

    private Stream getStream(IExtractable extractable)
        =>
        new ExtractorManager()
        .SetSource(extractable)
        .GetFeedStream();
    #endregion
}