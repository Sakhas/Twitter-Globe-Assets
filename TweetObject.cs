using UnityEngine;
using UnityEditor;

public class TweetObject {

    public static float MIN_LAT = -85f;
    public static float MAX_LAT = 85f;
    public static float MIN_LONG = -180f;
    public static float MAX_LONG = 180f;

    private Tweet tweet;
    private GameObject gameObject;

    public TweetObject(Tweet tweet) {
        this.tweet = tweet;
    }

    public Tweet getTweet()
    {
        return tweet;
    }

    public GameObject getGameObject()
    {
        return gameObject;
    }

    public void setGameObject (GameObject obj)
    {
        this.gameObject = obj;
        this.gameObject.AddComponent<OnHoverEffectScript>();
        OnHoverEffectScript script = this.gameObject.GetComponent<OnHoverEffectScript>();
        script.tweet = tweet;
    }

    public float getTweetLat()
    {
        float tweetLat = tweet.tweetLat;
        float placeLat = tweet.placeLat;

        if (tweetLat != 0)
        {
            return tweetLat;
        } else if (placeLat != 0)
        {
            return placeLat;
        } else
        {
            return addRandomLat();
        }
    }

    public float getTweetLong()
    {
        float tweetLong = tweet.tweetLong;
        float placeLong = tweet.placeLong;

        if (tweetLong != 0)
        {
            return tweetLong;
        }
        else if (placeLong != 0)
        {
            return placeLong;
        }
        else
        {
            return addRandomLong();
        }
    }

    private float addRandomLong()
    {
        return UnityEngine.Random.Range(MIN_LONG, MAX_LONG);
    }

    private float addRandomLat()
    {
        return UnityEngine.Random.Range(MIN_LAT, MAX_LAT);
    }
}