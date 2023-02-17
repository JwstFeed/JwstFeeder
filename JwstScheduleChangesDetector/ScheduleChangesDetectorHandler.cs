using Infrastructure.Logs;
using Infrastructure.Model;
using JwstScheduleChangesDetector.BL;
using JwstScheduleChangesDetector.Model;

namespace JwstScheduleChangesDetector;

public class ScheduleChangesDetectorHandler : IRunnable
{
    #region Data Members
    private IDalManager dalManager { get; }
    private string processName { get; }
    #endregion

    #region Ctor
    public ScheduleChangesDetectorHandler()
    {
        this.dalManager = DataAccessFactory.GetDalManagerObj();
        LogManager.AddLogger(logger: this.dalManager);
        this.processName = "ScheduleChangesDetector";
    }
    #endregion

    #region Public Methods
    public void Exec()
        =>
        ((Action)(() =>
        {
            string url = getLastProcessedUrl();
            handleScheduleChanges(url);
        }))
        .Catch((e) =>
        {
            return handleRetrievingScheduleException(e);
        });
    #endregion

    #region Private Methods
    private void handleScheduleChanges(string url)
    {
        ExpandedSchedule expandedSchedule = getExpandedSchedule(url);
        bool isScheduledChanged = detectChanges(expandedSchedule);

        if (isScheduledChanged)
        {
            updateDb(expandedSchedule);
        }
    }

    private ExpandedSchedule getExpandedSchedule(string url)
    {
        IReadOnlyCollection<Observation> uptodateObservations = getUptodateObservationSchedule(url);
        DateTime firstObservationStartTime = getFirstObservationStartTimeInSchedule(uptodateObservations);

        return new ExpandedSchedule()
        {
            CurrentObservations = getObservationSchedule(firstObservationStartTime),
            FirstObservationStartTimeInSchedule = firstObservationStartTime,
            UptodateObservations = uptodateObservations
        };
    }

    private void updateDb(ExpandedSchedule expandedSchedule)
    {
        deleteLastScheduleObervationsFromDb(expandedSchedule.FirstObservationStartTimeInSchedule);
        insertNewSchedule(expandedSchedule.UptodateObservations);
    }

    private IReadOnlyCollection<Observation> getUptodateObservationSchedule(string jwstCyclesUrl)
        =>
        new UrlProcessor()
        .GetObservationSchedule(jwstCyclesUrl)
        .ToList();

    private bool detectChanges(IChangesDetectableSchedule schedule)
        =>
        new ScheduleChangesDetector()
        .SetUptodateObservations(schedule.UptodateObservations)
        .SetCurrentObservations(schedule.CurrentObservations)
        .IsScheduleChanged();

    private string getLastProcessedUrl()
        =>
        this.dalManager
        .GetLastUrl();

    private IReadOnlyCollection<Observation> getObservationSchedule(DateTime firstObservationStartTime)
        =>
        this.dalManager
        .GetObservationSchedule(firstObservationStartTime);

    private void deleteLastScheduleObervationsFromDb(DateTime firstObservationStartTime)
        =>
        this.dalManager
        .DeleteLastSchedule(firstObservationStartTime);

    private void insertNewSchedule(IEnumerable<Observation> observationSchedule)
        =>
        this.dalManager
        .InsertUpdatedSchedule(observationSchedule);

    private void writeLog(string log)
        =>
        this.dalManager
        .WriteLog(log);

    private DateTime getFirstObservationStartTimeInSchedule(IEnumerable<Observation> observationSchedule)
        =>
        observationSchedule
        .OrderBy(o => o.ScheduledStartTime)
        .First()
        .ScheduledStartTime;

    private string handleRetrievingScheduleException(Exception ex)
        =>
        $"{this.processName} | Retrieving Urls | {ex.Message} | {ex.StackTrace}";
    #endregion
}