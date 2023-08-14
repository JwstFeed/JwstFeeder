using System.Globalization;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using Infrastructure.Utils;
using Newtonsoft.Json;

namespace Infrastructure.Extensions;

public static class ExtensionMethods
{
    #region Object
    public static string ToAbsString(this object? source)
    {
        return
            source?.ToString()
            ?? string.Empty;
    }

    public static int ToInt(this object source)
    {
        return source
            .ToAbsString()
            .ToInt();
    }

    public static T ToEnum<T>(this object source) where T : Enum
    {
        return (T)Enum.ToObject(typeof(T), source.ToInt());
    }
    #endregion

    #region Int
    public static bool Between(this int source, int n1, int n2)
    {
        return source >= n1 && source <= n2;
    }
    #endregion

    #region String
    public static string RemoveSpaces(this string source)
    {
        return source
            .Replace(" ", "");
    }

    public static string RemoveHtmlTabsAndNewLines(this string source)
    {
        return source
            .Replace("\t", string.Empty)
            .Replace("\n", string.Empty);
    }

    public static string GetStringBetweenTwoStrings(this string source, string str1, string str2)
    {
        int pFrom = source.IndexOf(str1) + str1.Length;
        int pTo = source.LastIndexOf(str2);

        return source[pFrom..pTo];
    }

    public static int ToInt(this string source)
    {
        return Convert.ToInt32(source);
    }

    public static double ToDouble(this string source)
    {
        return Convert.ToDouble(source);
    }

    public static string RemoveMeridiemIndicators(this string source)
    {
        return source
            .Replace("AM", string.Empty)
            .Replace("PM", string.Empty);
    }

    public static DateTime ToDateTime(this string source, string format)
    {
        return DateTime
            .ParseExact(source, format, CultureInfo.InvariantCulture);
    }

    public static DateTime FromUnixTime(this string source)
    {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        return dateTime
            .AddSeconds(source.ToDouble())
            .ToLocalTime();
    }

    public static DateTime FromMjdTime(this string source)
    {
        return DateTime
            .FromOADate((source.ToDouble() - 2415018.5) + 2400000.5);
    }

    public static string LastAppearanceAfter(this string source, char delimiter)
    {
        return source
            .Split(delimiter)
            .Last();
    }

    public static string LastAppearanceAfter(this string source, string delimiter)
    {
        return source
            .Split(delimiter)
            .Last();
    }

    public static string ToComparable(this string source)
    {
        return source
            .ToUpper()
            .Trim();
    }

    public static string GetContentBeforeFirstAppearanceOf(this string source, char delimiter)
    {
        return source
            .Split(delimiter)[0];
    }

    public static string GetContentBeforeFirstAppearanceOf(this string source, string delimiter)
    {
        return source
            .Split(delimiter)[0];
    }

    public static bool ContainsAnyOfTheFollowing(this string haystack, params string[] needles)
    {
        return needles
            .Any(haystack.Contains);
    }

    public static bool ContainsAllTheFollowing(this string haystack, params string[] needles)
    {
        return needles
            .All(haystack.Contains);
    }

    public static string ConcatBefore(this string after, string before)
    {
        return $"{before}{after}";
    }

    public static bool IsContainsHebrew(this string str)
    {
        char FirstHebChar = (char)1488;
        char LastHebChar = (char)1514;

        return str
            .Any(c => c >= FirstHebChar && c <= LastHebChar);
    }

    public static string DecodeHtmlSpecialChars(this string source)
    {
        return WebUtility.HtmlDecode(source);
    }

    public static string DeleteFirstLines(this string source, int numberOfRows)
    {
        return source
            .Split(Environment.NewLine.ToCharArray())
            .Skip(numberOfRows)
            .JoinString(Environment.NewLine);
    }

    public static Stream GetUrlStream(this string url)
    {
        return new JFWebClient()
        {
            TimeOut = GeneralUtils.GetAppSettingsInt("WebClientTimeOut")
        }
        .OpenRead(url);
    }
    #endregion

    #region String + Stream
    public static XDocument ToXDocument(this string source)
    {
        XmlDocument document = new();
        document.LoadXml(source);

        using XmlNodeReader reader = new(document);
        return XDocument.Load(reader, LoadOptions.None);
    }

    public static XDocument ToXDocument(this Stream source)
    {
        return source
            .ConvertToString()
            .ToXDocument();
    }

    public static T? Deserialize<T>(this string source)
    {
        return JsonConvert.DeserializeObject<T>(source);
    }

    public static T? Deserialize<T>(this Stream source)
    {
        string str = source.ConvertToString();

        return Deserialize<T>(str);
    }

    public static string ConvertToString(this Stream source)
    {
        return new StreamReader(source).ReadToEnd();
    }
    #endregion

    #region IEnumerable
    public static bool IsIn<T>(this T source, IEnumerable<T> arr)
    {
        return arr
            .Contains(source);
    }

    public static void ApplyEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (T item in enumeration)
        {
            action(item);
        }
    }

    public static IEnumerable<T> IterateEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (T item in enumeration)
        {
            action(item);
            yield return item;
        }
    }

    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        List<T> list = enumeration.ToList();

        for (int i = 0; i < list.Count; i++)
        {
            action(list[i]);
        }
    }

    public static string JoinString(this IEnumerable<string> source, string delimiter)
    {
        return string
            .Join(delimiter, source.ToArray());
    }
    #endregion

    #region XElement
    public static string GetElementValue(this XElement xElement, string elementName)
    {
        return xElement
            .Element(elementName)
            .Value;
    }

    public static string GetElementValue(this XElement xElement, XName elementName)
    {
        return xElement
            .Element(elementName)
            .Value;
    }
    #endregion

    #region DateTime
    public static string ToUnixTime(this DateTime source)
    {
        int unixTimeStamp = (int)source.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        return unixTimeStamp.ToString();
    }

    public static double ToMjdTime(this DateTime source)
    {
        return ((source.ToOADate() - 2400000.5) + 2415018.5);
    }

    public static DateTime AddArtificialHoursAndMinutes(this DateTime source)
    {
        return source
            .AddHours(DateTime.UtcNow.Hour)
            .AddMinutes(DateTime.UtcNow.Minute);
    }
    #endregion
        
    #region HashSet
    public static HashSet<T> AddRange<T>(this HashSet<T> source, HashSet<T> collectionToAdd)
    {
        source.UnionWith(collectionToAdd);

        return source;
    }
    #endregion
}
