namespace JwstScheduleChangesDetector.Model;

internal interface IChangesDetectableSchedule
{
    IReadOnlyCollection<Observation> UptodateObservations { get; }

    IReadOnlyCollection<Observation> CurrentObservations { get; }
}