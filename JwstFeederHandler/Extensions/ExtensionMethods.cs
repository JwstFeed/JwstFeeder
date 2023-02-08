using HtmlAgilityPack;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Extensions;

internal static class ExtensionMethods
{
    public static IEnumerable<RedditItemDetails> GetItems(this RedditItemModel source)
    {
        return source
            .Data
            .Items
            .Select(i => i.Data);
    }

    public static List<List<object>> GetRows(this StsciItemModel source)
    {
        return source
            .Data
            .Tables
            .FirstOrDefault()
            .Rows;
    }

    public static IEnumerable<YouTubeItem> GetVideos(this YouTubeItemModel source)
    {
        return source
            .Items;
    }

    public static string CleanHTML(this string source)
    {
        return source
            .Replace("&nbsp;", " ");
    }

    public static string FindAHrefValue(this HtmlNode htmlNode)
    {
        return htmlNode
            .FindInnerNode(nodeName: "a")
            .FindAttribute(attrName: "href")
            .Value;
    }

    public static HtmlNode FindInnerNode(this HtmlNode htmlNode, string nodeName)
    {
        return htmlNode
            .Descendants()
            .First(n => n.Name == nodeName);
    }

    public static HtmlAttribute FindAttribute(this HtmlNode htmlNode, string attrName)
    {
        return htmlNode
            .Attributes
            .First(a => a.Name == attrName);
    }

    public static string FindAttributeValue(this HtmlNode htmlNode, string attrName)
    {
        return htmlNode
            .FindAttribute(attrName)
            .Value;
    }

    public static int CastToInt(this eSourceType source)
    {
        return (int)source;
    }

    public static int CastToInt(this ePlotType source)
    {
        return (int)source;
    }
}