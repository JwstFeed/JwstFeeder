using System.Net;

namespace Infrastructure.Utils;

public class JFWebClient : WebClient
{
    /// <summary>
    /// set/get the time out in mSec
    /// </summary>
    public int TimeOut { get; set; }

    protected override WebRequest GetWebRequest(Uri uri)
    {
        WebRequest w = base.GetWebRequest(uri);
        w.Timeout = TimeOut;

        return w;
    }
}