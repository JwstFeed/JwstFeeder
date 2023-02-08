using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping.Model;

internal interface IMapper
{
    IMapper SetStream(Stream stream);

    IEnumerable<IFeedItem> Transform();
}