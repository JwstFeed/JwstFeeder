namespace JwstScheduleProvider.Model;

internal interface IObservation
{
    string ClusterIndex { get; }

    string VisitID { get; }

    string PcsMode { get; }

    string VisitType { get; }

    DateTime ScheduledStartTime { get; }

    string Duration { get; }

    string ScienceInstrumentAndMode { get; }

    string TargetName { get; }

    string Category { get; }

    string KeyWords { get; }
}
