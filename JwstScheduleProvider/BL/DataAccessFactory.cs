using JwstScheduleProvider.Model;

namespace JwstScheduleProvider.BL;

internal class DataAccessFactory
{
    public static IDalManager GetDalManagerObj()
        =>
        new EntityDalManager();
}