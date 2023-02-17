using System.Diagnostics.CodeAnalysis;

namespace JwstScheduleChangesDetector.Model;

internal class ShallowObservationComparer : IEqualityComparer<IComparableObservation>
{
    public bool Equals(IComparableObservation? obs1, IComparableObservation? obs2)
    {
        if (object.ReferenceEquals(obs1, obs2))
        {
            return true;
        }

        if (object.ReferenceEquals(obs1, null) || object.ReferenceEquals(obs2, null))
        {
            return false;
        }

        return obs1.ScheduledStartTime == obs2.ScheduledStartTime
            && obs1.TargetName == obs2.TargetName
            && obs1.VisitID == obs2.VisitID;
    }

    public int GetHashCode([DisallowNull] IComparableObservation obj)
        =>
        (obj.VisitID + obj.ScheduledStartTime.ToString())
        .GetHashCode();
}