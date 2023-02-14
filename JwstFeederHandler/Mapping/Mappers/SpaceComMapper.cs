using HtmlAgilityPack;
using Infrastructure.Extensions;
using Infrastructure.Utils;
using JwstFeederHandler.Extensions;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class SpaceComMapper : IMapper
{
    #region Data Members
    private Stream stream { get; set; }
    private static string mainArticlesNodesClass { get; } = "listingResults ";
    private static string dateFormat { get; } = "yyyy-MM-ddTHH:mm:ssZ";
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
        IEnumerable<HtmlNode> articles = getArticles(doc);

        return articles
            .Select(a => new FeedItem()
            {
                DatePublished = getPublishDate(a),
                ThumbnailUrl = getThumbnailUrl(a),
                SourceType = eSourceType.SpaceCom,                
                ShortTitle = getShortTitle(a),
                PlotType = ePlotType.Image,
                SourceUrl = getPlotUrl(a),
                PlotUrl = getPlotUrl(a),
            })
            .IterateEach(i =>
            {
                i.ClusterIndex = getClusterIndex(i);
                i.UniqueID = getUniqueID(i);
            })
            .Where(i =>
            {
                return isJwstRelevant(i);
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

    private string getClusterIndex(IFeedItem item)
    {
        string unixPublishDate = item.DatePublished.ToUnixTime();
        string uniqueID = getUniqueID(item);

        return $"{unixPublishDate}_{uniqueID}";
    }

    private bool isJwstRelevant(IFeedItem item)
    {
        bool isTitleContainsRelevantWords = this.isTitleContainsRelevantWords(item);
        bool isIdContainsRelevantWords = this.isIdContainsRelevantWords(item);

        return isTitleContainsRelevantWords || isIdContainsRelevantWords;
    }

    private bool isTitleContainsRelevantWords(IFeedItem item)
        =>
        item
        .ShortTitle
        .ToUpper()
        .ContainsAnyOfTheFollowing(GeneralUtils.GetAppSettingsArr("RelevantJwstWords"));

    private bool isIdContainsRelevantWords(IFeedItem item)
        =>
        item
        .UniqueID
        .ToUpper()
        .ContainsAnyOfTheFollowing(GeneralUtils.GetAppSettingsArr("RelevantJwstWords"));

    private DateTime getPublishDate(HtmlNode node)
        =>
        node
        .FindInnerNode(nodeName: "time")
        .FindAttributeValue(attrName: "datetime")
        .ToDateTime(dateFormat);

    private string getPlotUrl(HtmlNode node)
        =>
        node
        .FindAHrefValue();

    private string getShortTitle(HtmlNode node)
        =>
        node
        .FindInnerNode(nodeName: "h3")
        .InnerText
        .DecodeHtmlSpecialChars();

    private string getThumbnailUrl(HtmlNode node)
        =>
        node
        .FindInnerNode(nodeName: "figure")
        .FindAttributeValue("data-original");

    private string getUniqueID(IFeedItem item)
        =>
        item
        .PlotUrl
        .LastAppearanceAfter('/');

    private IEnumerable<HtmlNode> getArticles(HtmlDocument doc)
        =>
        doc
        .DocumentNode
        .SelectNodes($"//div[@class='{mainArticlesNodesClass}']")
        .Descendants()
        .Where(isDivNode)
        .Where(isSearchResult);

    private bool isDivNode(HtmlNode node)
        =>
        node.Name == "div";

    private bool isSearchResult(HtmlNode node)
        =>
        node
        .FindAttributeValue(attrName: "class")
        .Contains("listingResult");    
    #endregion
}
