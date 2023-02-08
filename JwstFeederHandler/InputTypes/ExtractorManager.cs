using JwstFeederHandler.InputTypes.Extractors;
using JwstFeederHandler.InputTypes.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.InputTypes;

internal class ExtractorManager
{
    #region Data Members
    private string url { get; set; }
    private eInputType inputType { get; set; }
    #endregion

    #region Public Methods
    public ExtractorManager SetSource(IExtractable source)
    {
        this.url = source.Url;
        this.inputType = source.InputType;

        return this;
    }

    public Stream GetFeedStream()
    {
        IExtractor extractor = getExtractor();

        return extractor
          .GetExternalStream();
    }
    #endregion

    #region Private Methods
    private IExtractor getExtractor()
        =>
        this.inputType switch
        {
            eInputType.ByUsername => new ByUsernameExtractor(this.url),
            eInputType.MastPortal => new MastPortalExtractor(this.url),
            eInputType.HttpPost => new HttpBodyExtractor(this.url),
            eInputType.HttpGet => new HttpExtractor(this.url),
            _ => throw new Exception("No Extractor Found")
        };
    #endregion
}