using JwstScheduleProvider.Extensions;
using JwstScheduleProvider.Model;

namespace JwstScheduleProvider.BL;

internal class UrlProcessor
{
    #region Data Members
    private ObservationGenerator observationGenerator { get; }
    private int realTimeCommandColumnsNumber { get; }
    private int multiObjectColumnsNumber { get; }
    private int validColumnsNumber { get; }
    #endregion

    #region Ctor
    public UrlProcessor()
    {
        this.observationGenerator = new ObservationGenerator();
        this.realTimeCommandColumnsNumber = 7;
        this.multiObjectColumnsNumber = 8;
        this.validColumnsNumber = 9;
    }
    #endregion

    #region Public Methods
    public IEnumerable<IObservation> GetObservationSchedule(string url)
        =>
        url
        .GetUrlContent()
        .ToScheduleTable()
        .Select(parseObservation)
        .Where(isNotEmptyObservation);
    #endregion

    #region Private Methods
    private IObservation parseObservation(string observation)
    {
        string[] observationRow = observation.ToScheduleTableRow();

        return observationRow.Length switch
        {
            int value when value == this.realTimeCommandColumnsNumber => getRealTimeCommandObservation(observationRow),
            int value when value == this.multiObjectColumnsNumber => getMultiObjectObservation(observationRow),
            int value when value == this.validColumnsNumber => getNormalObservation(observationRow),
            _ => new EmptyObservation()
        };
    }

    private IObservation getRealTimeCommandObservation(string[] observationRow)
        =>
        this.observationGenerator
        .GenerateRealTimeCommandObservation(observationRow);

    private IObservation getMultiObjectObservation(string[] observationRow)
        =>
        this.observationGenerator
        .GenerateMultiObjectObservation(observationRow);

    private IObservation getNormalObservation(string[] observationRow)
        =>
        this.observationGenerator
        .GenerateNormalObservation(observationRow);

    private bool isNotEmptyObservation(IObservation observation)
        =>
        observation is not EmptyObservation;
    #endregion
}
