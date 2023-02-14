using Newtonsoft.Json;

namespace JwstFeederHandler.Mapping.Model;

internal class RedditItemModel
{
    [JsonProperty(PropertyName = "data")]
    public RedditItemDetails Data { get; set; }
    
    public IEnumerable<RedditItemDetails> GetItems()
        =>
        Data
        .Items
        .Select(i => i.Data);
}

internal class RedditItem
{
    [JsonProperty(PropertyName = "data")]
    public RedditItemDetails Data { get; set; }
}

internal class RedditItemDetails
{
    [JsonProperty(PropertyName = "created_utc")]
    public string DateCreated { get; set; }

    [JsonProperty(PropertyName = "children")]
    public IEnumerable<RedditItem> Items { get; set; }

    [JsonProperty(PropertyName = "id")]
    public string ID { get; set; }

    [JsonProperty(PropertyName = "is_video")]
    public bool IsVideo { get; set; }

    [JsonProperty(PropertyName = "permalink")]
    public string Permalink { get; set; }

    [JsonProperty(PropertyName = "title")]
    public string Title { get; set; }

    [JsonProperty(PropertyName = "ups")]
    public int Upvotes { get; set; }

    [JsonProperty(PropertyName = "url")]
    public string URL { get; set; }
}
