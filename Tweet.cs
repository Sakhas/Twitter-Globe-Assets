using UnityEngine;
using UnityEditor;
using System;
using System.Globalization;

[System.Serializable]
public class Tweet
{

    public int id;
    public string text;
    public string userLocation;
    public string placeCountry;
    public string placeName;
    public float placeLong;
    public float placeLat;
    public string userName;
    public float tweetLong;
    public float tweetLat;
    public DateTime created;

    public Tweet()
    {

    } 

    public int getId()
    {
        return this.id;
    }

    public override string ToString()
    {
        return "Tweet: " + id + " " + text + " " +  " " + userLocation + " " + placeLat + " " + created + " " + placeLat ;
    }

}