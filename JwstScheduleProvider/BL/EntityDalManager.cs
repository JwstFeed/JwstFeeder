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

    public void InsertNewSchedule(IEnumerable<IObservation> observations)
    {
        IEnumerable<Observation> entityObservations = observations
            .Select(o => new Observation()
            {
                ScienceInstumentAndMode = o.ScienceInstumentAndMode,
                ScheduledStartTime = o.ScheduledStartTime,
                ClusterIndex = o.ClusterIndex,
                TargetName = o.TargetName,
                VisitType = o.VisitType,
                KeyWords = o.KeyWords,
                Category = o.Category,
                Duration = o.Duration,
                VisitID = o.VisitID,
                PcsMode = o.PcsMode
            });

        this.dbContext
            .Observations
            .AddRange(entityObservations);

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
