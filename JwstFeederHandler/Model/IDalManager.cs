using Infrastructure.Logs;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Model;

internal interface IDalManager : ILogger
{
    IEnumerable<FeedSource> GetFeedSources();

    HashSet<string> GetExistingKeys(eSourceType sourceType);

    void InsertNewItems(IEnumerable<IFeedItem> items);
}