using Newtonsoft.Json;

namespace JwstFeederHandler.Mapping.Model;

public class StsciItemModel
{
    [JsonProperty(PropertyName = "data")]
    public StsciItem Data { get; set; }
}

public class StsciItem
{
    [JsonProperty(PropertyName = "Tables")]
    public List<StsciTableItem> Tables { get; set; }
}

public class StsciTableItem
{
    [JsonProperty(PropertyName = "Rows")]
    public List<List<object>> Rows { get; set; }
}