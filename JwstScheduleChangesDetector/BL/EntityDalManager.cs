using Infrastructure.Extensions;
using JwstScheduleChangesDetector.DAL;
using JwstScheduleChangesDetector.DAL.EntityModels;
using JwstScheduleChangesDetector.Model;

namespace JwstScheduleChangesDetector.BL;

internal class EntityDalManager : IDalManager
{
    #region Data Members
    private ScheduleChangesDetectorDbContext dbContext { get; } = new ScheduleChangesDetectorDbContext();
    #endregion

    #region Public Methods
    public void DeleteLastSchedule(DateTime firstObservationDate)
    {
        IQueryable<Observation> observationsToDelete = this.dbContext
            .Observations
            .Where(o => o.ScheduledStartTime >= firstObservationDate);

        this.dbContext
            .RemoveRange(observationsToDelete);

        this.dbContext
            .SaveChanges();
    }

    public string GetLastUrl()
    {
        var urls = this.dbContext
            .ScheduleUrls
            .ToList();

        return urls
            .OrderByDescending(u => u.Url.LastAppearanceAfter('_'))
            .First()
            .Url;
    }

    public IReadOnlyCollection<Observation> GetObservationSchedule(DateTime firstObservationDate)
    {
        IReadOnlyCollection<Observation> observationSchedule = this.dbContext
            .Observations
            .Where(o => o.ScheduledStartTime >= firstObservationDate)
            .ToList();

        return observationSchedule;
    }

    public void InsertUpdatedSchedule(IEnumerable<Observation> observations)
    {
        this.dbContext
            .Observations
            .AddRange(observations);

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