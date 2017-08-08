using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace WPM
{

    public class DrawTweetMarkerScript : MonoBehaviour
    {

        public static string TWEET_URL = "http://localhost:8080/tweets/";

        public List<TweetObject> tweetObjects = new List<TweetObject>();

        public WorldMapGlobe map;
        public GUIStyle labelStyle, labelStyleShadow;
        public float kmRadius = 20;
        public float ringWidthStart = 0;
        public float ringWidthEnd = 1.0f;

        private float time = 0.0f;
        public float interpolationPeriod = 5f;

        GameObject tweet_icon;

        void Start()
        {
            labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.white;
            labelStyleShadow = new GUIStyle(labelStyle);
            labelStyleShadow.normal.textColor = Color.black;

            GUIResizer.Init(800, 500);

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

        void OnGUI()
        {

            // Do autoresizing of GUI layer
            GUIResizer.AutoResize();
            if (map.mouseIsOver)
            {
                string text;
                Vector3 mousePos = Input.mousePosition;
                float x, y;
                if (map.countryHighlighted != null || map.cityHighlighted != null)
                {
                    City city = map.cityHighlighted;
                    if (city != null)
                    {
                        if (city.province != null && city.province.Length > 0)
                        {
                            text = "City: " + map.cityHighlighted.name + " (" + city.province + ", " + map.countries[map.cityHighlighted.countryIndex].name + ")";
                        }
                        else
                        {
                            text = "City: " + map.cityHighlighted.name + " (" + map.countries[map.cityHighlighted.countryIndex].name + ")";
                        }
                    }
                    else if (map.countryHighlighted != null)
                    {
                        text = map.countryHighlighted.name + " (" + map.countryHighlighted.continent + ")";
                    }
                    else
                    {
                        text = "";
                    }
                    x = GUIResizer.authoredScreenWidth * (mousePos.x / Screen.width);
                    y = GUIResizer.authoredScreenHeight - GUIResizer.authoredScreenHeight * (mousePos.y / Screen.height) - 20 * (Input.touchSupported ? 3 : 1); // slightly up for touch devices
                    GUI.Label(new Rect(x, y, 0, 10), text, labelStyle);
                }
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

        string EntityListToString<T>(List<T> entities)
        {
            StringBuilder sb = new StringBuilder("Neighbours: ");
            for (int k = 0; k < entities.Count; k++)
            {
                if (k > 0)
                {
                    sb.Append(", ");
                }
                sb.Append(((IAdminEntity)entities[k]).name);
            }
            return sb.ToString();
        }

        public void updateTweets()
        {
            Tweet[] tweets = getTweetData();
            int firstId = tweets[tweets.Length - 1].id;

            tweetObjects.Sort((x, y) => y.getTweet().id.CompareTo(x.getTweet().id));
            int maxId = tweetObjects[0].getTweet().id;

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
            return tweets;
        }

       public void addMarker(TweetObject tweet)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Vector3 sphereLocation = Conversion.GetSpherePointFromLatLon(new Vector2(tweet.getTweetLat(), tweet.getTweetLong()));
            //GameObject obj = map.AddMarker(MARKER_TYPE.CIRCLE, sphereLocation, kmRadius, ringWidthStart, ringWidthEnd, Color.green);
            map.AddMarker(cube, sphereLocation, 0.001f);

            tweet.setGameObject(cube);
            
            
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