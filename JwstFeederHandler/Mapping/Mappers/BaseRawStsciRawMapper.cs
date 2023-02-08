using Infrastructure.Extensions;
using Infrastructure.Utils;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Mappers;

internal abstract class BaseRawStsciRawMapper : IMapper
{
    #region Data Members
    protected IDictionary<string, int> indexKeys { get; }
    protected string[] irrelevantImageNameWords { get; }
    protected string[] testImageWords { get; }
    protected string[] irrelevantFilters { get; }
    protected string[] irrelevantNrcas { get; }
    protected Stream stream { get; set; }
    #endregion

    #region Ctor
    public BaseRawStsciRawMapper()
    {
        this.irrelevantImageNameWords = new string[]
        {
            "_cal",
            "_uncal",
            "_dark",
            "shortmediumlong",
            "shortmedium",
            "_mirifu"
        };

        this.testImageWords = new string[]
        {
            "ROUTINE",
            "CYCLE"
        };

        this.irrelevantFilters = new string[]
        {
            "F212N"
        };

        this.irrelevantNrcas = new string[]
        {
            "nrca2",
            "nrca4"
        };

        this.indexKeys = new Dictionary<string, int>()
        {
            { "Instrument", 3 },
            { "Filters", 5 },
            { "Waveband", 6 },
            { "TargetName", 7 },
            { "ObsID", 9 },
            { "StartTime", 15 },
            { "ObsTime", 17 },
            { "ObsTitle", 20 },
            { "ReleaseDate", 21 },
            { "JpgUrl", 26 },
            { "FitsUrl", 27 },
            { "DataRights", 28 },
        };
    }
    #endregion

    #region Public Methods
    public IMapper SetStream(Stream stream)
    {
        this.stream = stream;

        return this;
    }
    #endregion

    #region Abstract Methods
    public abstract IEnumerable<IFeedItem> Transform();

    protected abstract bool isRelevantImage(List<object> obj);

    protected abstract eSourceType getSourceType(List<object> obj);
    #endregion

    #region Protected Methods
    protected string getClusterIndex(List<object> obj)
    {
        string unixPublishDate = getReleasedDate(obj).ToUnixTime();
        string uniqueID = getUniqueID(obj);

        return $"{unixPublishDate}_{uniqueID}";
    }

    protected bool isNotExclusiveImage(List<object> obj)
        =>
        obj[indexKeys["DataRights"]].ToAbsString() != "EXCLUSIVE_ACCESS";

    protected string getInstrumentName(List<object> obj)
        =>
        obj[indexKeys["Instrument"]]
        .ToAbsString()
        .ToComparable();

    protected bool isNotCalibrationImage(List<object> obj)
        =>
        !obj[indexKeys["JpgUrl"]]
        .ToAbsString()
        .ContainsAnyOfTheFollowing(this.irrelevantImageNameWords);

    protected bool isIrrelevantNrca(List<object> obj)
        =>
        obj[indexKeys["JpgUrl"]]
        .ToAbsString()
        .ContainsAnyOfTheFollowing(this.irrelevantNrcas);

    protected bool isTestImage(List<object> obj)
        =>
        obj[indexKeys["ObsTitle"]]
        .ToAbsString()
        .ToUpper()
        .ContainsAllTheFollowing(this.testImageWords);

    protected bool isIrrelevantFilter(List<object> obj)
        =>
        obj[indexKeys["Filters"]]
        .ToAbsString()
        .ToUpper()
        .ContainsAnyOfTheFollowing(this.irrelevantFilters);

    protected string getMqImagePath(List<object> obj)
        =>
        obj[indexKeys["JpgUrl"]]
        .ToAbsString()
        .Split(':')
        .Last()
        .ConcatBefore(GeneralUtils.GetAppSettings("StsciImageUrl"));

    protected string getShortTitle(List<object> obj)
        =>
        string.Format("TargetName: {1}{0}Title: {2}{0} Instrument: {3}{0}Filters: {4}{0}Waveband: {5}{0}Start Time: {6}{0}Obs Time: {7}(s)",
            Environment.NewLine,
            obj[indexKeys["TargetName"]].ToAbsString(),
            obj[indexKeys["ObsTitle"]].ToAbsString(),
            obj[indexKeys["Instrument"]].ToAbsString(),
            obj[indexKeys["Filters"]].ToAbsString(),
            obj[indexKeys["Waveband"]].ToAbsString(),
            getStartTime(obj).ToString(),
            obj[indexKeys["ObsTime"]].ToAbsString());

    protected string getUniqueID(List<object> obj)
        =>
        obj[indexKeys["ObsID"]].ToAbsString();

    protected DateTime getStartTime(List<object> obj)
        =>
        obj[indexKeys["StartTime"]]
        .ToAbsString()
        .GetContentBeforeFirstAppearanceOf('.')
        .FromMjdTime();

    protected DateTime getReleasedDate(List<object> obj)
        =>
        obj[indexKeys["ReleaseDate"]]
        .ToAbsString()
        .GetContentBeforeFirstAppearanceOf('.')
        .FromMjdTime()
        .AddArtificialHoursAndMinutes();
    #endregion
}