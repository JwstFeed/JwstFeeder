using Newtonsoft.Json;

namespace JwstFeederHandler.Mapping.Model;

public class YouTubeItemModel
{
    [JsonProperty(PropertyName = "items")]
    public List<YouTubeItem> Items { get; set; }
    
    public IEnumerable<YouTubeItem> GetVideos() => Items;
}

public class YouTubeItem
{
    [JsonProperty(PropertyName = "id")]
    public VideoIdInfo VideoIdInfo { get; set; }

    [JsonProperty(PropertyName = "snippet")]
    public Snippet Snippet { get; set; }
}

public class VideoIdInfo
{
    [JsonProperty(PropertyName = "videoId")]
    public string VideoID { get; set; }
}

public class Snippet
{
    [JsonProperty(PropertyName = "publishedAt")]
    public DateTime PublishedAt { get; set; }

    [JsonProperty(PropertyName = "title")]
    public string VideoTitle { get; set; }

    [JsonProperty(PropertyName = "channelTitle")]
    public string ChannelTitle { get; set; }
}
