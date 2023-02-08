using Infrastructure.Extensions;
using JwstScheduleProvider.Extensions;
using JwstScheduleProvider.Model;

namespace JwstScheduleProvider.BL;

internal class UrlProcessor
{
    #region Data Members
    private static string dateFormat { get; } = "yyyy-MM-dTHH:mm:ssZ";
    private IDictionary<string, int> keys { get; }
    private int validRowNumber { get; }
    #endregion

    #region Ctor
    public UrlProcessor()
    {
        this.validRowNumber = 9;

        this.keys = new Dictionary<string, int>()
        {
            { "VisitID", 0 },
            { "PcsMode", 1 },
            { "VisitType", 2 },
            { "ScheduledStartTime", 3 },
            { "Duration", 4 },
            { "ScienceInstumentAndMode", 5 },
            { "TargetName", 6 },
            { "Category", 7 },
            { "KeyWords", 8 },
        };
    }
    #endregion

    #region Public Methods
    public IEnumerable<Observation> GetObservationSchedule(string url)
        =>
        url
        .GetUrlContent()
        .ToScheduleTable()
        .Select(parseObservation)
        .Where(isRowNotEmpty);
    #endregion

    #region Private Methods
    private Observation parseObservation(string observation)
    {
        string[] observationRow = observation.ToScheduleTableRow();
        bool isValidScheduleRow = observationRow.Length == this.validRowNumber;

        return isValidScheduleRow
            ? new Observation()
            {
                ScienceInstumentAndMode = observationRow[keys["ScienceInstumentAndMode"]],
                ScheduledStartTime = getScheduledStartTime(observationRow),
                TargetName = observationRow[keys["TargetName"]],
                ClusterIndex = getClusterIndex(observationRow),
                VisitType = observationRow[keys["VisitType"]],
                Category = observationRow[keys["Category"]],
                Duration = observationRow[keys["Duration"]],
                KeyWords = observationRow[keys["KeyWords"]],
                PcsMode = observationRow[keys["PcsMode"]],
                VisitID = getVisitID(observationRow)
            }
            : new Observation();
    }

    private string getClusterIndex(string[] observationRow)
    {
        string unixStartTime = getScheduledStartTime(observationRow).ToUnixTime();
        string visitID = getVisitID(observationRow);

        return $"{unixStartTime}_{visitID}";
    }

    private DateTime getScheduledStartTime(string[] observationRow)
        =>
        observationRow[keys["ScheduledStartTime"]]
        .ToDateTime(dateFormat);

    private string getVisitID(string[] observationRow)
        =>
        observationRow[keys["VisitID"]];

    private bool isRowNotEmpty(Observation observation)
        =>
        observation.TargetName != null;
    #endregion
}