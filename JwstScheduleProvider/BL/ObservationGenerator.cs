using Infrastructure.Extensions;
using JwstScheduleProvider.Model;

namespace JwstScheduleProvider.BL;

internal class ObservationGenerator
{
    #region Data Members
    private static string dateFormat { get; } = "yyyy-MM-dTHH:mm:ssZ";
    private IDictionary<string, int> keys { get; }
    #endregion

    #region Ctor
    public ObservationGenerator()
    {
        this.keys = new Dictionary<string, int>()
        {
            { "VisitID", 0 },
            { "PcsMode", 1 },
            { "VisitType", 2 },
            { "ScheduledStartTime", 3 },
            { "Duration", 4 },
            { "ScienceInstrumentAndMode", 5 },
            { "TargetName", 6 },
            { "Category", 7 },
            { "KeyWords", 8 },
        };
    }
    #endregion

    #region Public Methods
    public IObservation GenerateMultiObjectObservation(string[] observationRow)
        =>
        new MultiObjectObservation()
        {
            ScienceInstrumentAndMode = observationRow[keys["ScienceInstrumentAndMode"]],
            ScheduledStartTime = getScheduledStartTime(observationRow),
            TargetName = observationRow[keys["TargetName"]],
            ClusterIndex = getClusterIndex(observationRow),
            VisitType = observationRow[keys["VisitType"]],
            Duration = observationRow[keys["Duration"]],
            PcsMode = observationRow[keys["PcsMode"]],
            VisitID = getVisitID(observationRow)
        };

    public IObservation GenerateNormalObservation(string[] observationRow)
        =>
        new Observation()
        {
            ScienceInstrumentAndMode = observationRow[keys["ScienceInstrumentAndMode"]],
            ScheduledStartTime = getScheduledStartTime(observationRow),
            TargetName = observationRow[keys["TargetName"]],
            ClusterIndex = getClusterIndex(observationRow),
            VisitType = observationRow[keys["VisitType"]],
            Category = observationRow[keys["Category"]],
            Duration = observationRow[keys["Duration"]],
            KeyWords = observationRow[keys["KeyWords"]],
            PcsMode = observationRow[keys["PcsMode"]],
            VisitID = getVisitID(observationRow)
        };

    public IObservation GenerateRealTimeCommandObservation(string[] observationRow)
        =>
        new RealTimeCommandObservation()
        {
            ScienceInstrumentAndMode = observationRow[keys["ScienceInstrumentAndMode"]],
            ScheduledStartTime = getScheduledStartTime(observationRow),
            ClusterIndex = getClusterIndex(observationRow),
            VisitType = observationRow[keys["VisitType"]],
            Duration = observationRow[keys["Duration"]],
            PcsMode = observationRow[keys["PcsMode"]],
            VisitID = getVisitID(observationRow)
        };
    #endregion

    #region Private Methods
    private string getClusterIndex(string[] observationRow)
    {
        string unixStartTime = getScheduledStartTime(observationRow).ToUnixTime();
        string visitID = getVisitID(observationRow);

        return $"{unixStartTime}_{visitID}";
    }

    private DateTime getScheduledStartTime(string[] observationRow)
        =>
        observationRow[keys["ScheduledStartTime"]]
        .ToDateTime(dateFormat)
        .ToUniversalTime();

    private string getVisitID(string[] observationRow)
        =>
        observationRow[keys["VisitID"]];
    #endregion
}
