using Infrastructure.Extensions;
using Infrastructure.Utils;
using JwstFeederHandler.Mapping.Model;
using JwstFeedInfrastructure.Model;
using TwitterSharp.Client;
using TwitterSharp.Request.AdvancedSearch;
using TwitterSharp.Request.Option;
using TwitterSharp.Response.RTweet;
using TwitterSharp.Response.RUser;

namespace JwstFeederHandler.Mapping.Mappers;

internal class TwitterMapper : IMapper
{
    #region Data Members
    private TwitterClient twitterClient { get; }
    private string userName { get; set; }
    private int maxRequestLimit { get; }
    private string twitterRawPhotoBotName { get; }
    #endregion

    #region Ctor
    public TwitterMapper()
    {
        this.twitterClient = new TwitterClient(GeneralUtils.GetAppSettings("TwitterBearerToken"));
        this.maxRequestLimit = 100;
        this.twitterRawPhotoBotName = "jwstphotobot";
    }
    #endregion

    #region Public Methods
    public IMapper SetStream(Stream stream)
    {
        this.userName = stream.ConvertToString();

        return this;
    }

    public IEnumerable<IFeedItem> Transform()
    {
        IEnumerable<Tweet> tweets = getTweets();

        return tweets
            .Where(isJwstRelevant)
            .Where(isNotHebrewPost)
            .Where(hasAttachments)
            .Select(t => new FeedItem()
            {
                DatePublished = getDatePublished(t),
                ClusterIndex = getClusterIndex(t),
                SourceType = getSourceType(),
                ThumbnailUrl = getPlotUrl(t),
                SourceUrl = getSourceUrl(t),
                ShortTitle = getShortTitle(t),
                PlotType = getPlotType(t),
                PlotUrl = getPlotUrl(t),
                UniqueID = t.Id
            })
            .Where(i => i.PlotUrl != null);
    }
    #endregion

    #region Private Methods
    private string getClusterIndex(Tweet tweet)
    {
        string unixPublishDate = getDatePublished(tweet).ToUnixTime();
        string uniqueID = tweet.Id;

        return $"{unixPublishDate}_{uniqueID}";
    }

    private IEnumerable<Tweet> getTweets()
    {
        string userID = getUserId()
            .Result
            .Id;

        return getTweetsByUserId(userID)
            .Result;
    }

    private Task<User> getUserId()
        =>
        this.twitterClient
        .GetUserAsync(userName);

    private Task<Tweet[]> getTweetsByUserId(string userID)
        =>
        this.twitterClient
        .GetTweetsFromUserIdAsync(userID, new TweetSearchOptions
        {
            Limit = this.maxRequestLimit,
            TweetOptions = new[]
            {
                TweetOption.Attachments,
                TweetOption.Created_At
            },
            MediaOptions = new[]
            {
                MediaOption.Url
            }
        });

    private bool isJwstRelevant(Tweet tweet)
        =>
        tweet
        .Text
        .ToUpper()
        .ContainsAnyOfTheFollowing(GeneralUtils.GetAppSettingsArr("RelevantJwstWords"));

    private bool isNotHebrewPost(Tweet tweet)
        =>
        !tweet
        .Text
        .IsContainsHebrew();

    private bool hasAttachments(Tweet tweet)
        =>
        tweet.Attachments != null
        && (tweet.Attachments?.Media?.Any() ?? false);

    private ePlotType getPlotType(Tweet tweet)
        =>
        ePlotType.Image;

    private string getSourceUrl(Tweet tweet)
        =>
        tweet
        .Text
        .LastAppearanceAfter(GeneralUtils.GetAppSettings("TwitterShortUrl"))
        .ConcatBefore(GeneralUtils.GetAppSettings("TwitterShortUrl"));

    private string getPlotUrl(Tweet tweet)
        =>
        tweet
        .Attachments
        .Media
        .First()
        .Url;

    private string getShortTitle(Tweet tweet)
        =>
        tweet
        .Text
        .GetContentBeforeFirstAppearanceOf(GeneralUtils.GetAppSettings("TwitterShortUrl"));

    private eSourceType getSourceType()
        =>
        this.userName.ToComparable() == this.twitterRawPhotoBotName.ToComparable()
        ? eSourceType.TwitterRawPhotoBot
        : eSourceType.Twitter;

    private DateTime getDatePublished(Tweet tweet)
        =>
        tweet.CreatedAt ?? DateTime.Now;
    #endregion
}