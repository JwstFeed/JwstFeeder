using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Model;

internal class FeedItem : IFeedItem
{
    public string ClusterIndex { get; set; }

    public DateTime DatePublished { get; set; } = DateTime.Today;

    public ePlotType PlotType { get; set; }

    public string PlotUrl { get; set; }

    public string ShortTitle { get; set; }

    public eSourceType SourceType { get; set; }

    public string SourceUrl { get; set; }

    public string UniqueID { get; set; }

    public string ThumbnailUrl { get; set; } = string.Empty;
}