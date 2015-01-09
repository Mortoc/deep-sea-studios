using UnityEngine;
using System;


public interface IReceipt : IDisposable
{
}

public class SimpleReceipt : IReceipt
{
    private Action _cleanup;

    public SimpleReceipt(Action cleanup)
    {
        _cleanup = cleanup;
    }

    public void Dispose()
    {
        if( _cleanup!= null )
            _cleanup();
    }
}


public class EchoNest 
{
    //private static string API_KEY = "ELBDM05MHACH7A5TG";
    //private static string SECRET_KEY = "eS+wSejEQFSCqcQptrq8aQ";
    //private static string CUSTOMER_KEY = "fd3df52703c186be59202f614534d0a3";

    public struct TrackInfo
    {

    }

    public static IReceipt GetTrackInfo(string trackName, string artistName, Action<TrackInfo> callback)
    {
        return new SimpleReceipt(null);
    }
}
