using JwstScheduleChangesDetector.Model;

namespace JwstScheduleChangesDetector.BL;

internal class DataAccessFactory
{
    public static IDalManager GetDalManagerObj()
        =>
        new EntityDalManager();

    public static IEqualityComparer<IComparableObservation> GetComparerObj()
        =>
        new ShallowObservationComparer();
}