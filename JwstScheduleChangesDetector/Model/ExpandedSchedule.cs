namespace JwstScheduleChangesDetector.Model;

internal class ExpandedSchedule : IChangesDetectableSchedule
{
    public IReadOnlyCollection<Observation> UptodateObservations { get; set; }

    public IReadOnlyCollection<Observation> CurrentObservations { get; set; }

    public DateTime FirstObservationStartTimeInSchedule { get; set; }
}