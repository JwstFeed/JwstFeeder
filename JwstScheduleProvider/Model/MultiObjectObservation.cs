namespace JwstScheduleProvider.Model;

internal class MultiObjectObservation : IObservation
{
    public string ClusterIndex { get; set; }

    public string VisitID { get; set; }

    public string PcsMode { get; set; }

    public string VisitType { get; set; }

    public DateTime ScheduledStartTime { get; set; }

    public string Duration { get; set; }

    public string ScienceInstrumentAndMode { get; set; }

    public string TargetName { get; set; }

    public string Category => this.ScienceInstumentAndMode;

    public string KeyWords => this.ScienceInstumentAndMode;
}
