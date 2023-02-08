using JwstFeederHandler.Model;

namespace JwstFeederHandler.BL;

internal class DataAccessFactory
{
    public static IDalManager GetDalManagerObj()
        =>
        new EntityDalManager();
}