using JwstFeederHandler.InputTypes.Model;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Model;

internal class FeedSource : IExtractable, IMappable
{
    public eInputType InputType { get; set; }

    public eSourceType SourceType { get; set; }

    public string Url { get; set; }
}