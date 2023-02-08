using JwstScheduleProvider.DAL;
using JwstScheduleProvider.DAL.EntityModels;
using JwstScheduleProvider.Model;

namespace JwstScheduleProvider.BL;

internal class EntityDalManager : IDalManager
{
    #region Data Members
    private ScheduleDbContext dbContext { get; } = new ScheduleDbContext();
    #endregion

    #region
    public IEnumerable<string> GetUrls()
    {
        List<string> urls = this.dbContext
            .ScheduleUrls
            .Where(u => !u.IsProcessed)
            .Select(u => u.Url)
            .ToList();

        return urls;
    }

    public void InsertNewSchedule(IEnumerable<Observation> observations)
    {
        this.dbContext
            .Observations
            .AddRange(observations);

        this.dbContext
            .SaveChanges();
    }

    public void MarkUrlAsDone(string url)
    {
        this.dbContext
            .ScheduleUrls
            .First(u => u.Url == url)
            .IsProcessed = true;

        this.dbContext
            .SaveChanges();
    }

    public void WriteLog(string log)
    {
        EntityLogModel entityLog = new EntityLogModel()
        {
            Log = log
        };

        this.dbContext
            .ScheduleProcessingLogs
            .Add(entityLog);

        this.dbContext
            .SaveChanges();
    }
    #endregion
}