using System.Xml.Linq;
using Infrastructure.Extensions;
using Infrastructure.Utils;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class FlickrMapper : IMapper
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
        .ToXDocument()
        .Elements()
        .Elements("photos")
        .Elements("photo")
        .Where(isJwstRelevant)
        .Select(e => new FeedItem()
        {
            ThumbnailUrl = getMediumImageUrl(e),
            ShortTitle = e.Attribute("title").Value,
            ClusterIndex = getClusterIndex(e),
            SourceType = eSourceType.Flickr,
            SourceUrl = getSourceUrl(e),
            PlotType = ePlotType.Image,
            UniqueID = getUniqueID(e),
            PlotUrl = getPlotUrl(e)
        });
    #endregion

    #region Private Methods
    private string getClusterIndex(XElement element)
    {
        string unixPublishDate = DateTime.Now.ToUnixTime();
        string uniqueID = getUniqueID(element);

        return $"{unixPublishDate}_{uniqueID}";
    }

    private string getPlotUrl(XElement element)
    {
        string flickrOriginalImagePath = GeneralUtils.GetAppSettings("FlickrOriginalImagePath");

        return $"{flickrOriginalImagePath}/{element.Attribute("id").Value}_{element.Attribute("secret").Value}_b.jpg";
    }

    private string getMediumImageUrl(XElement element)
    {
        string flickrThumbnailImagePath = GeneralUtils.GetAppSettings("FlickrOriginalImagePath");

        return $"{flickrThumbnailImagePath}/{element.Attribute("id").Value}_{element.Attribute("secret").Value}_z.jpg";
    }

    private string getSourceUrl(XElement element)
    {
        string flickrWebbPagePath = GeneralUtils.GetAppSettings("FlickrWebbPagePath");

        return $"{flickrWebbPagePath}/{element.Attribute("id").Value}/";
    }

    private string getUniqueID(XElement element)
        =>
        element
        .Attribute("id")
        .Value;

    private bool isJwstRelevant(XElement element)
        =>
        !element
        .Attribute("title")
        .Value
        .ToUpper()
        .ContainsAnyOfTheFollowing(GeneralUtils.GetAppSettingsArr("IrrelevantJwstWords"));
    #endregion
}