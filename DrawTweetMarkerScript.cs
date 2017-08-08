using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace WPM
{

    public class DrawTweetMarkerScript : MonoBehaviour
    {

        public static string TWEET_URL = "http://localhost:8080/tweets/";

        public List<TweetObject> tweetObjects = new List<TweetObject>();

        WorldMapGlobe map;
        public float kmRadius = 20;
        public float ringWidthStart = 0;
        public float ringWidthEnd = 1.0f;

        private float time = 0.0f;
        public float interpolationPeriod = 5f;

        GameObject tweet_icon;

        void Start()
        {
            tweet_icon = GameObject.Find("twitter");
            map = WorldMapGlobe.instance;
            Tweet[] tweets = getTweetData();

            foreach (Tweet tweet in tweets)
            {
                TweetObject obj = new TweetObject(tweet);
                tweetObjects.Add(obj);
                addMarker(obj);
            }
        }

        private void Update()
        {
            time += Time.deltaTime;

            if (time >= interpolationPeriod)
            {
                time = time - interpolationPeriod;
                updateTweets();
            }
        }

        public void updateTweets()
        {
            Tweet[] tweets = getTweetData();
            int maxId = tweets[0].id;
            int firstId = tweets[tweets.Length - 1].id;
            print("firstId: " +  firstId);
            print("maxId: " + maxId);

            updateTweetObjects(maxId, firstId, tweets);
        }

        private void updateTweetObjects(int maxId, int firstId, Tweet[] tweets)
        {
            foreach (TweetObject tweet in tweetObjects.ToArray()) {
                if(tweet.getTweet().getId() < firstId)
                {
                    deleteMarker(tweet);
                }
            }

            foreach (Tweet tweet in tweets)
            {
                if(tweet.getId() > maxId)
                {
                    TweetObject obj = new TweetObject(tweet);
                    tweetObjects.Add(obj);
                    addMarker(obj);
                }
            }
        }

        public Tweet[] getTweetData()
        {
            Uri tweetUri = new Uri(TWEET_URL);
            WebRequest getRequest;
            getRequest = WebRequest.Create(tweetUri);
            Stream objStream;
            objStream = getRequest.GetResponse().GetResponseStream();
           
            String result;
            StreamReader reader = new StreamReader(objStream);
            result = fixJson(reader.ReadToEnd());

            Tweet[] tweets = JsonHelper.FromJson<Tweet>(result);
            print(tweets.Length);
            print(tweets[0]);
            return tweets;
        }

       public void addMarker(TweetObject tweet)
        {
            Vector3 sphereLocation = Conversion.GetSpherePointFromLatLon(new Vector2(tweet.getTweetLat(), tweet.getTweetLong()));
            GameObject obj = map.AddMarker(MARKER_TYPE.CIRCLE_PROJECTED, sphereLocation, kmRadius, ringWidthStart, ringWidthEnd, Color.green);
            tweet.setGameObject(obj);
            
        }
        
        public void deleteMarker(TweetObject obj)
        {
            Destroy(obj.getGameObject());
            tweetObjects.Remove(obj);
        }

        public string fixJson(string value)
        {
            value = "{\"Items\":" + value + "}";
            return value;
        }

        GameObject createNewGameObject()
        {
            GameObject obj = null;
            return obj;
        }
    }

}