using Infrastructure.Logs;

namespace JwstScheduleProvider.Model;

internal interface IDalManager : ILogger
{
    IEnumerable<string> GetUrls();

    void InsertNewSchedule(IEnumerable<Observation> observations);

    void MarkUrlAsDone(string url);
}