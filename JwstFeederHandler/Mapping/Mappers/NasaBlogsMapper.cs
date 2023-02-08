using HtmlAgilityPack;
using Infrastructure.Extensions;
using JwstFeederHandler.Extensions;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class NasaBlogsMapper : IMapper
{
    #region Data Members
    private Stream stream { get; set; }
    private static string dateFormat { get; } = "MMMM d, yyyy";
    #endregion

    #region Public Methods
    public IMapper SetStream(Stream stream)
    {
        this.stream = stream;

        return this;
    }

    public IEnumerable<IFeedItem> Transform()
    {
        HtmlDocument doc = getHtmlDocument();

        var relevantNodes = doc
            .DocumentNode
            .SelectNodes($"//article");

        return relevantNodes
            .Select(n => new FeedItem()
            {
                SourceType = eSourceType.NasaBlogs,
                DatePublished = getPublishDate(n),
                ClusterIndex = getClusterIndex(n),
                SourceUrl = getArticleUrl(n),
                UniqueID = getUniqueID(n),
                PlotType = ePlotType.Link,
                PlotUrl = getArticleUrl(n),
                ShortTitle = getTitle(n)
            });
    }
    #endregion

    #region Private Methods
    private HtmlDocument getHtmlDocument()
    {
        HtmlDocument doc = new HtmlDocument();
        doc.Load(this.stream);

        return doc;
    }

    private string getClusterIndex(HtmlNode node)
    {
        string unixPublishDate = getPublishDate(node).ToUnixTime();
        string uniqueID = getUniqueID(node);

        return $"{unixPublishDate}_{uniqueID}";
    }

    private DateTime getPublishDate(HtmlNode node)
        =>
        node
        .FindInnerNode(nodeName: "time")
        .InnerText
        .ToDateTime(dateFormat)
        .AddArtificialHoursAndMinutes();

    private string getTitle(HtmlNode node)
        =>
        node
        .FindInnerNode(nodeName: "h2")
        .InnerText;

    private string getArticleUrl(HtmlNode node)
        =>
        node
        .FindAHrefValue();

    private string getUniqueID(HtmlNode node)
        =>
        node
        .FindAttributeValue(attrName: "id");
    #endregion
}