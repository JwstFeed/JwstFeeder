using Infrastructure.Extensions;
using Infrastructure.Utils;
using JwstFeederHandler.Extensions;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class RedditMapper : IMapper
{
    #region Data Members
    private int minimunUpvotesTreshold { get; }
    private Stream stream { get; set; }
    #endregion

    #region Ctor
    public RedditMapper()
    {
        this.minimunUpvotesTreshold = 5;
    }
    #endregion

    #region Public Methods
    public IMapper SetStream(Stream stream)
    {
        this.stream = stream;

        return this;
    }

    public IEnumerable<IFeedItem> Transform()
        =>
        this.stream
        .Deserialize<RedditItemModel>()
        .GetItems()
        .Where(isJwstRelevant)
        .Where(isNotSpam)
        .Select(i => new FeedItem()
        {
            SourceUrl = $"{GeneralUtils.GetAppSettings("RedditUrl")}{i.Permalink}",
            DatePublished = i.DateCreated.FromUnixTime(),
            ClusterIndex = getClusterIndex(i),
            SourceType = eSourceType.Reddit,            
            PlotType = getPlotType(i),
            PlotUrl = getPlotUrl(i),
            ThumbnailUrl = i.URL,
            ShortTitle = i.Title,
            UniqueID = i.ID
        });
    #endregion

    #region Private Methods
    private string getClusterIndex(RedditItemDetails redditItem)
    {
        string unixPublishDate = redditItem.DateCreated;
        string uniqueID = redditItem.ID;

        return $"{unixPublishDate}_{uniqueID}";
    }

    private bool isNotSpam(RedditItemDetails redditItem)
    {
        bool isUpvoted = redditItem.Upvotes >= this.minimunUpvotesTreshold;
        bool isContainsIrrelevantWords = redditItem.Title.ToUpper().ContainsAllTheFollowing("TATTOO");

        return isUpvoted && !isContainsIrrelevantWords;
    }

    private ePlotType getPlotType(RedditItemDetails redditItem)
        =>
        redditItem.URL.ContainsAnyOfTheFollowing(".jpg", ".png", ".gif")
        ? ePlotType.Image
        : (redditItem.IsVideo ? ePlotType.Video : ePlotType.Link);

    private bool isJwstRelevant(RedditItemDetails redditItem)
        =>
        redditItem
        .Title
        .ToUpper()
        .ContainsAnyOfTheFollowing(GeneralUtils.GetAppSettingsArr("RelevantJwstWords"));        

    private string getPlotUrl(RedditItemDetails redditItem)
        =>
        redditItem.IsVideo
        ? $"{redditItem.URL}/DASH_720.mp4"
        : redditItem.URL;
    #endregion
}
