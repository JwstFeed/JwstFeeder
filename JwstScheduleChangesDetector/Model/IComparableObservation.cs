namespace JwstScheduleChangesDetector.Model;

internal interface IComparableObservation
{
    string VisitID { get; }

    DateTime ScheduledStartTime { get; }

    string TargetName { get; }
}