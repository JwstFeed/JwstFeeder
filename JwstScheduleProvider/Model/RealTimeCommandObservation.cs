namespace JwstScheduleProvider.Model;

internal class RealTimeCommandObservation : IObservation
{
    public string ClusterIndex { get; set; }

    public string VisitID { get; set; }

    public string PcsMode { get; set; }

    public string VisitType { get; set; }

    public DateTime ScheduledStartTime { get; set; }

    public string Duration { get; set; }

    public string ScienceInstumentAndMode { get; set; }

    public string TargetName => this.ScienceInstumentAndMode;

    public string Category => this.ScienceInstumentAndMode;

    public string KeyWords => this.ScienceInstumentAndMode;
}
