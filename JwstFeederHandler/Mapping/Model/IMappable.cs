using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Model;

internal interface IMappable
{
    eSourceType SourceType { get; }
}