namespace Infrastructure.Logs;

public interface ILogger
{
    void WriteLog(string log);
}