using Infrastructure.Extensions;
using JwstFeederHandler.Extensions;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal class StsciRawFiltereredOutMapper : StsciRawMapper
{
    #region Public Overridden Methods
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
        bool isCalibrationImage = base.isCalibrationImage(obj);
        bool isTestImage = base.isTestImage(obj);
        bool isIrrelevantFilter = base.isIrrelevantFilter(obj);
        bool isIrrelevantNrca = base.isIrrelevantNrca(obj);

        return isCalibrationImage
            || (isTestImage && isIrrelevantFilter && isIrrelevantNrca);
    }

    protected override eSourceType getSourceType(List<object> obj)
        =>
        eSourceType.StsciRawFilteredOut;
    #endregion
}
