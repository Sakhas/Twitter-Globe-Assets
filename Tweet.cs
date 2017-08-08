using UnityEngine;
using UnityEditor;
using System;

[System.Serializable]
public class Tweet
{

    private int id;
    private string text;
    private string userLocation;
    private string placeCountry;
    private string placeName;
    private string placeLong;
    private string placeLat;
    private string userName;
    private string tweetLong;
    private string tweetLat;
    private DateTime created;

    public Tweet()
    {

    } 

    public int getId()
    {
        return this.id;
    }


}