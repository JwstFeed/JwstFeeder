using System.ComponentModel.DataAnnotations;

namespace JwstFeederHandler.DAL.EntityModels;

internal class EntityFeedItemModel
{
    [Key]
    public string ClusterIndex { get; set; }

    public DateTime DatePublished { get; set; } = DateTime.Today;

    public bool IsDirty { get; set; } = false;

    public int PlotTypeID { get; set; }

    public string PlotUrl { get; set; }

    public string ShortTitle { get; set; }

    public int SourceTypeID { get; set; }

    public string SourceUrl { get; set; }

    public string ThumbnailUrl { get; set; } = string.Empty;

    public string UniqueID { get; set; }
}