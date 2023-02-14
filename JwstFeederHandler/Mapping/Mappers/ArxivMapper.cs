using System.Xml.Linq;
using Infrastructure.Extensions;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class ArxivMapper : IMapper
{
    #region Data Members
    private Stream stream { get; set; }
    private static XNamespace ns { get; } = "http://www.w3.org/2005/Atom";
    private static string dateFormat { get; } = "yyyy-MM-ddTHH:mm:ssZ";
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
        .ToXDocument()
        .Elements()
        .Elements(ns + "entry")
        .Select(e => new FeedItem()
        {
            ShortTitle = e.Element(ns + "title").Value,
            DatePublished = getPublishDate(e),
            ClusterIndex = getClusterIndex(e),
            SourceType = eSourceType.Arxiv,
            SourceUrl = getArticleUrl(e),            
            PlotUrl = getArticleUrl(e),            
            PlotType = ePlotType.Link,
            UniqueID = getUniqueID(e)
        });
    #endregion

    #region Private Methods
    private string getClusterIndex(XElement node)
    {
        string unixPublishDate = getPublishDate(node).ToUnixTime();
        string uniqueID = getUniqueID(node);

        return $"{unixPublishDate}_{uniqueID}";
    }

    private DateTime getPublishDate(XElement node)
        =>
        node
        .GetElementValue(elementName: ns + "published")
        .ToDateTime(dateFormat);

    private string getArticleUrl(XElement node)
        =>
        node
        .GetElementValue(elementName: ns + "id")
        .Replace("/abs/", "/pdf/");

    private string getUniqueID(XElement node)
        =>
        node
        .GetElementValue(elementName: ns + "id")
        .LastAppearanceAfter('/');
    #endregion
}
