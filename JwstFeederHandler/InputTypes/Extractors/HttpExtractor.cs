using Infrastructure.Extensions;
using JwstFeederHandler.InputTypes.Model;

namespace JwstFeederHandler.InputTypes.Extractors;

internal class HttpExtractor : IExtractor
{
    #region Data Members
    private string inputUrl { get; }
    #endregion

    #region Ctor
    public HttpExtractor(string inputUrl)
    {
        this.inputUrl = inputUrl;
    }
    #endregion

    public Stream GetExternalStream()
        =>
        this.inputUrl
        .GetUrlStream();
}