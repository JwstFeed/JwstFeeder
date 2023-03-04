using Infrastructure.Extensions;
using JwstFeederHandler.Extensions;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class StsciRawMapper : BaseRawStsciRawMapper
{
    #region Public Overriden Methods
    public override IEnumerable<IFeedItem> Transform()
        =>
        base.stream
        .Deserialize<StsciItemModel>()
        .GetRows()
        .Where(isNotExclusiveImage)
        .Where(isRelevantImage)
        .Select(i => new FeedItem()
        {
            DatePublished = getReleasedDate(i),
            ClusterIndex = getClusterIndex(i),
            ThumbnailUrl = getMqImagePath(i),            
            SourceType = getSourceType(i),
            ShortTitle = getShortTitle(i),
            PlotUrl = getMqImagePath(i),
            PlotType = ePlotType.Image,            
            UniqueID = getUniqueID(i),
            SourceUrl = "#"
        });
    #endregion

    #region Protected Overridden Methods
    protected override bool isRelevantImage(List<object> obj)
    {
        bool isNotCalibrationImage = base.isNotCalibrationImage(obj);
        bool isTestImage = base.isTestImage(obj);
        bool isIrrelevantFilter = base.isIrrelevantFilter(obj);
        bool isIrrelevantNrca = base.isIrrelevantNrca(obj);

        return isNotCalibrationImage
            && !(isTestImage && isIrrelevantFilter && isIrrelevantNrca);
    }

    protected override eSourceType getSourceType(List<object> obj)
    {
        string instrumentName = base.getInstrumentName(obj);

        return instrumentName switch
        {
            string x when x.Contains("NIRSPEC") => eSourceType.StsciRawNirspec,
            string x when x.Contains("NIRISS") => eSourceType.StsciRawNiriss,
            string x when x.Contains("NIRCAM") => eSourceType.StsciRawNircam,
            string x when x.Contains("MIRI") => eSourceType.StsciRawMiri,
            _ => throw new Exception("No Instrument Found")
        };
    }
    #endregion
}
