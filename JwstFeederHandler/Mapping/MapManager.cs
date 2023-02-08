using JwstFeederHandler.Mapping.Mappers;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;

namespace JwstFeederHandler.Mapping;

internal class MapManager
{
    #region Data Members
    private IMappable mappable { get; set; }
    private Stream stream { get; set; }
    #endregion

    #region Ctor	
    public MapManager()
    {
        this.stream = Stream.Null;
    }
    #endregion

    #region Public Methods
    public MapManager SetMappable(IMappable mappable)
    {
        this.mappable = mappable;

        return this;
    }

    public MapManager SetStream(Stream stream)
    {
        this.stream = stream;

        return this;
    }

    public IEnumerable<IFeedItem> GetFeedItems()
    {
        IMapper mapper = getMapper();

        return mapper
            .SetStream(stream)
            .Transform();
    }
    #endregion

    #region Private Methods
    private IMapper getMapper()
        =>
        this.mappable.SourceType switch
        {
            eSourceType.WebbTelescopeImagesOrg => new WebbTelescopeImagesMapper(),
            eSourceType.WebbTelescopeOrg => new WebbTelescopeMapper(),
            eSourceType.StsciSchedule => new StsciScheduleMapper(),
            eSourceType.TwitterRawPhotoBot => new TwitterMapper(),
            eSourceType.StsciRawNirspec => new StsciRawMapper(),
            eSourceType.StsciRawNircam => new StsciRawMapper(),
            eSourceType.StsciRawNiriss => new StsciRawMapper(),
            eSourceType.StsciRawMiri => new StsciRawMapper(),
            eSourceType.NasaBlogs => new NasaBlogsMapper(),
            eSourceType.EsaWebb => new EsaWebbMapper(),
            eSourceType.Youtube => new YouTubeMapper(),
            eSourceType.Twitter => new TwitterMapper(),
            eSourceType.Reddit => new RedditMapper(),
            eSourceType.Flickr => new FlickrMapper(),
            eSourceType.Arxiv => new ArxivMapper(),
            _ => throw new Exception("No Mapper Found")
        };
    #endregion
}