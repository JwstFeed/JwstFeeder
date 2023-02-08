namespace Infrastructure.Logs;

public static class LogExtensions
{
    #region Action
    public static void Catch(this Action sourceAction, Func<Exception, string> catchAction)
    {
        try
        {
            sourceAction();
        }
        catch (Exception ex)
        {
            string log = catchAction(ex);

            foreach (ILogger logger in LogManager.Loggers)
            {
                logger.WriteLog(log);
            }
        }
    }
    #endregion
}