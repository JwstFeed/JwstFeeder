namespace Infrastructure.Logs;

public static class LogManager
{
    #region Properties
    public static ICollection<ILogger> Loggers => _loggers;
    #endregion

    #region Data Members
    private static ICollection<ILogger> _loggers { get; set; } = new List<ILogger>();
    #endregion

    #region Public Methods
    public static void AddLogger(ILogger logger)
    {
        _loggers.Add(logger);
    }

    public static void CleanLoggers()
    {
        _loggers.Clear();
    }
    #endregion
}