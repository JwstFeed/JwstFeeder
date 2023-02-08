using Infrastructure.Extensions;
using Infrastructure.Logs;
using Infrastructure.Model;
using JwstScheduleProvider.BL;
using JwstScheduleProvider.Model;

namespace JwstScheduleProvider;

public class ScheduleProviderHandler : IRunnable
{
    #region Data Members
    private IDalManager dalManager { get; }
    private string processName { get; }
    #endregion

    #region Ctor
    public ScheduleProviderHandler()
    {
        this.dalManager = DataAccessFactory.GetDalManagerObj();
        LogManager.AddLogger(logger: this.dalManager);
        this.processName = "ScheduleProvider";
    }
    #endregion

    #region Public Methods
    public void Exec()
        =>
        ((Action)(() =>
        {
            IEnumerable<string> urls = getUrls();
            processUrls(urls);
            LogManager.CleanLoggers();
        }))
        .Catch((e) =>
        {
            return handleRetrievingUrlsException(e);
        });
    #endregion

    #region Private Methods
    private void processUrls(IEnumerable<string> urls)
        =>
        urls
        .ForEach(processSingleUrl);

    private void processSingleUrl(string url)
        =>
        ((Action)(() =>
        {
            IEnumerable<Observation> observationSchedule = getObservationSchedule(url);
            insertNewSchedule(observationSchedule);
            markUrlAsDone(url);
        }))
        .Catch((e) =>
        {
            return handleUrlProcessException(url, e);
        });

    private string handleUrlProcessException(string url, Exception ex)
        =>
        $"{this.processName} | {url} | {ex.Message} | {ex.StackTrace}";

    private string handleRetrievingUrlsException(Exception ex)
        =>
        $"{this.processName} | Retrieving Urls | {ex.Message} | {ex.StackTrace}";

    private IEnumerable<string> getUrls()
        =>
        this.dalManager
        .GetUrls();

    private void insertNewSchedule(IEnumerable<Observation> observations)
        =>
        this.dalManager
        .InsertNewSchedule(observations);

    private void markUrlAsDone(string url)
        =>
        this.dalManager
        .MarkUrlAsDone(url);

    private IEnumerable<Observation> getObservationSchedule(string url)
        =>
        new UrlProcessor()
        .GetObservationSchedule(url);
    #endregion
}