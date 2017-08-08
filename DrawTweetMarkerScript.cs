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
        public static float MIN_LAT = -85f;
        public static float MAX_LAT = 85f;
        public static float MIN_LONG = -180f;
        public static float MAX_LONG = 180f;

        public List<TweetObject> tweetObjects = new List<TweetObject>();

        WorldMapGlobe map;
        float kmRadius = 50;
        float ringWidthStart = 0;
        float ringWidthEnd = 1.0f;

        private float time = 0.0f;
        public float interpolationPeriod = 30f;

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
            int firstId = tweets[0].getId();
            tweetObjects.Sort((x, y) => x.getTweet().getId().CompareTo(y.getTweet().getId()));
            int maxId = tweetObjects[0].getTweet().getId();

            updateTweetObjects(maxId, firstId, tweets);
        }

        private void updateTweetObjects(int maxId, int firstId, Tweet[] tweets)
        {
            foreach (TweetObject tweet in tweetObjects) {
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
                    addMarker(addRandomLat(), addRandomLong(), obj);
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

            print(result);

            Tweet[] tweets = JsonHelper.FromJson<Tweet>(result);
            return tweets;
        }

        private float addRandomLong()
        {
            return UnityEngine.Random.Range(MIN_LONG, MAX_LONG);
        }

        private float addRandomLat()
        {
            return UnityEngine.Random.Range(MIN_LAT, MAX_LAT);
        }

       public void addMarker(float lat, float longitude, TweetObject tweet)
        {
            Vector3 sphereLocation = Conversion.GetSpherePointFromLatLon(new Vector2(lat, longitude));
            print(sphereLocation);
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