using System.Globalization;
using HtmlAgilityPack;
using Infrastructure.Extensions;
using Infrastructure.Utils;
using JwstFeederHandler.Extensions;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class StsciScheduleMapper : IMapper
{
    #region Data Members
    private Stream stream { get; set; }
    public static string dateFormat { get; } = "MMM d, yyyy";
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

        HtmlNode relevantNode = doc
            .DocumentNode
            .SelectNodes($"//div[@id='{GeneralUtils.GetAppSettings("StsciScheduleDivID")}']")
            .First();

        return relevantNode
            .Descendants()
            .Where(isAHrefNode)
            .Where(isNotPreviousSchedule)
            .Select(d => new FeedItem()
            {
                SourceType = eSourceType.StsciSchedule,
                ClusterIndex = getClusterIndex(d),
                DatePublished = getStartDate(d),
                UniqueID = getUniqueID(d),
                SourceUrl = getPlotUrl(d),
                PlotType = ePlotType.Link,
                ShortTitle = getTitle(d),
                PlotUrl = getPlotUrl(d)
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
        string unixPublishDate = getStartDate(node).ToUnixTime();
        string uniqueID = getUniqueID(node);

        return $"{unixPublishDate}_{uniqueID}";
    }

    private string getPlotUrl(HtmlNode node)
    {
        string partialUrl = getPartialUrl(node);

        return $"{GeneralUtils.GetAppSettings("StsciUrl")}/{partialUrl}";
    }

    private DateTime getStartDate(HtmlNode node)
    {
        return DateTime.UtcNow;
    }

    private string getTitle(HtmlNode node)
    {
        string cleanTitle = getCleanTitle(node);

        return $"Schedule: {cleanTitle}";
    }

    private string getUniqueID(HtmlNode node)
    {
        string cleanTitle = getCleanTitle(node);

        return cleanTitle
            .RemoveSpaces();
    }

    private string getCleanTitle(HtmlNode node)
        =>
        node
        .InnerHtml
        .CleanHTML();

    private string getPartialUrl(HtmlNode node)
        =>
        node
        .FindAttributeValue(attrName: "href");

    private bool isAHrefNode(HtmlNode node)
        =>
        node.Name == "a";

    private bool isNotPreviousSchedule(HtmlNode node)
        =>
        node.InnerHtml != "previous"
        && !node.InnerHtml.Contains('/');
    #endregion
}