using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.InputTypes.Model;

internal interface IExtractable
{
    eInputType InputType { get; }

    string Url { get; }
}