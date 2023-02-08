using System.Text;
using JwstFeederHandler.InputTypes.Model;

namespace JwstFeederHandler.InputTypes.Extractors;

internal class ByUsernameExtractor : IExtractor
{
    #region Data Members
    private string userName { get; }
    #endregion

    #region Ctor
    public ByUsernameExtractor(string userName)
    {
        this.userName = userName;
    }
    #endregion

    public Stream GetExternalStream()
        =>
        new MemoryStream(Encoding.UTF8.GetBytes(this.userName));
}