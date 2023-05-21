using System.ComponentModel.DataAnnotations;

namespace JwstScheduleProvider.Model;

internal class Observation : IObservation
{
    [Key]
    public string ClusterIndex { get; set; }

    public string VisitID { get; set; }

    public string PcsMode { get; set; }

    public string VisitType { get; set; }

    public DateTime ScheduledStartTime { get; set; }

    public string Duration { get; set; }

    public string ScienceInstumentAndMode { get; set; }

    public string TargetName { get; set; }

    public string Category { get; set; }

    public string KeyWords { get; set; }
}
