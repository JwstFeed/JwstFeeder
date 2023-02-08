using Infrastructure.Extensions;
using Infrastructure.Utils;
using JwstFeederHandler.Extensions;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class YouTubeMapper : IMapper
{
    #region Data Members
    private Stream stream { get; set; }
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
        .Deserialize<YouTubeItemModel>()
        .GetVideos()
        .Where(isValidVideo)
        .Where(isJwstRelevant)
        .Select(v => new FeedItem()
        {
            SourceUrl = getVideoEmbedUrl(v).Replace("embed/", "watch?v="),
            DatePublished = getDatePublished(v),
            SourceType = eSourceType.Youtube,
            ClusterIndex = getClusterIndex(v),
            PlotUrl = getVideoEmbedUrl(v),
            PlotType = ePlotType.Video,
            UniqueID = getUniqueID(v),
            ShortTitle = getTitle(v)
        });
    #endregion

    #region Private Methods
    private string getClusterIndex(YouTubeItem item)
    {
        string unixPublishDate = getDatePublished(item).ToUnixTime();
        string uniqueID = getUniqueID(item);

        return $"{unixPublishDate}_{uniqueID}";
    }

    private string getTitle(YouTubeItem item)
    {
        string title = $"{item.Snippet.ChannelTitle} | {item.Snippet.VideoTitle}";

        return title
            .DecodeHtmlSpecialChars();
    }

    private string getVideoEmbedUrl(YouTubeItem item)
        =>
        $"{GeneralUtils.GetAppSettings("YouTubeEmbedUrl")}/{item.VideoIdInfo.VideoID}";

    private bool isValidVideo(YouTubeItem item)
        =>
        !string.IsNullOrEmpty(item.VideoIdInfo.VideoID);

    private bool isJwstRelevant(YouTubeItem item)
        =>
        item
        .Snippet
        .VideoTitle
        .ToUpper()
        .ContainsAnyOfTheFollowing(GeneralUtils.GetAppSettingsArr("RelevantJwstWords"));

    private DateTime getDatePublished(YouTubeItem item)
        =>
        item
        .Snippet
        .PublishedAt;

    private string getUniqueID(YouTubeItem item)
        =>
        item
        .VideoIdInfo
        .VideoID;
    #endregion
}