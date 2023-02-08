using Infrastructure.Model;
using JwstFeederHandler;
using JwstScheduleProvider;

namespace FeederRunner;

public static class Program
{
    public static void Main(string[] args)
        =>
        new List<IRunnable>()
        {
            new FeederHandler(),
            new ScheduleProviderHandler()
        }
        .ForEach(r =>
        {
            r.Exec();
        });
}