using Infrastructure.Extensions;

namespace JwstScheduleProvider.Extensions;

internal static class ExtensionMethods
{
    public static string[] ToScheduleTable(this string source)
    {
        return source
            .DeleteFirstLines(numerOfRows: 4)
            .Split('\n');
    }

    public static string[] ToScheduleTableRow(this string source)
    {
        const string delimiter = "#unique#";

        return source
            .Replace("  ", delimiter)
            .Split(delimiter)
            .Where(r => !string.IsNullOrEmpty(r))
            .Select(r => r.Trim())
            .ToArray();
    }

    public static string GetUrlContent(this string source)
    {
        return source
            .GetUrlStream()
            .ConvertToString();
    }
}
