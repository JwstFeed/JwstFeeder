using JwstScheduleChangesDetector.Model;

namespace JwstScheduleChangesDetector.BL;

internal class ScheduleChangesDetector
{
    #region Private Methods
    private IReadOnlyCollection<IComparableObservation> uptodateObsevationSchedule { get; set; }
    private IReadOnlyCollection<IComparableObservation> currentObservationsSchedule { get; set; }
    private IEqualityComparer<IComparableObservation> comparer { get; }
    #endregion

    #region Ctor
    public ScheduleChangesDetector()
    {
        this.comparer = DataAccessFactory.GetComparerObj();
    }
    #endregion

    #region Public Methods
    public ScheduleChangesDetector SetUptodateObservations(IReadOnlyCollection<IComparableObservation> observations)
    {
        this.uptodateObsevationSchedule = observations;

        return this;
    }

    public ScheduleChangesDetector SetCurrentObservations(IReadOnlyCollection<IComparableObservation> observations)
    {
        this.currentObservationsSchedule = observations;

        return this;
    }

    public bool IsScheduleChanged()
        =>
        !(this.currentObservationsSchedule.Count == this.uptodateObsevationSchedule.Count
        &&
        Enumerable.SequenceEqual(this.currentObservationsSchedule,
                                 this.uptodateObsevationSchedule,
                                 this.comparer));
    #endregion
}