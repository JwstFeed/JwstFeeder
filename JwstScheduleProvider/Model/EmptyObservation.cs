namespace JwstScheduleProvider.Model;

internal class EmptyObservation : IObservation
{
    public string ClusterIndex => string.Empty;

    public string VisitID => string.Empty;

    public string PcsMode => string.Empty;

    public string VisitType => string.Empty;

    public DateTime ScheduledStartTime => DateTime.UtcNow;

    public string Duration => string.Empty;

    public string ScienceInstrumentAndMode => string.Empty;

    public string TargetName => string.Empty;

    public string Category => string.Empty;

    public string KeyWords => string.Empty;
}
