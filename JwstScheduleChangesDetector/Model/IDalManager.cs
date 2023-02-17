using Infrastructure.Logs;

namespace JwstScheduleChangesDetector.Model;

internal interface IDalManager : ILogger
{
    IReadOnlyCollection<Observation> GetObservationSchedule(DateTime firstObservationDate);

    string GetLastUrl();

    void DeleteLastSchedule(DateTime firstObservationDate);

    void InsertUpdatedSchedule(IEnumerable<Observation> observations);
}