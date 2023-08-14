namespace JwstScheduleProvider.Model;

internal class RealTimeCommandObservation : IObservation
{
    public string ClusterIndex { get; set; }

    public string VisitID { get; set; }

    public string PcsMode { get; set; }

    public string VisitType { get; set; }

    public DateTime ScheduledStartTime { get; set; }

    public string Duration { get; set; }

    public string ScienceInstrumentAndMode { get; set; }

    public string TargetName => this.ScienceInstrumentAndMode;

    public string Category => this.ScienceInstrumentAndMode;

    public string KeyWords => this.ScienceInstrumentAndMode;
}
