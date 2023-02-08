using Infrastructure.Extensions;
using JwstFeederHandler.InputTypes.Model;

namespace JwstFeederHandler.InputTypes.Extractors;

internal class MastPortalExtractor : IExtractor
{
    #region Data Members
    private string inputUrl { get; }
    #endregion

    #region Ctor
    public MastPortalExtractor(string inputUrl)
    {
        this.inputUrl = inputUrl;
    }
    #endregion

    #region Public Methods
    public Stream GetExternalStream()
    {
        string lastWeekDateMjd = getLastWeekMjdTime();
        string nextWeekDateMjd = getNextWeekMjdTime();

        return this.inputUrl
            .Replace("{{start-date}}", lastWeekDateMjd)
            .Replace("{{end-date}}", nextWeekDateMjd)
            .GetUrlStream();
    }
    #endregion

    #region Private Methods
    private string getLastWeekMjdTime()
        =>
        DateTime.Today
        .AddDays(-7)
        .ToMjdTime()
        .ToString();

    private string getNextWeekMjdTime()
        =>
        DateTime.Today
        .AddDays(7)
        .ToMjdTime()
        .ToString();

    private string getYearsAgoMjdTime()
        =>
        DateTime.Today
        .AddYears(-3)
        .ToMjdTime()
        .ToString();

    private string getYearFromNowMjdTime()
        =>
        DateTime.Today
        .AddYears(1)
        .ToMjdTime()
        .ToString();
    #endregion
}