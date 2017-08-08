using UnityEngine;
using UnityEditor;

public class TweetObject {

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
    }

}