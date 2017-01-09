using System.Collections;
using com.rmc.projects.event_dispatcher;
using UnityEngine;

public class CaptureEvent : com.rmc.projects.event_dispatcher.Event
{

    public static string ON_CAPTURE_SUCCESS = "OnCaptureSuccess";
    public static string ON_CAPTURE_FAIL = "OnCaptureFail";
    public static string ON_CAPTURE_START = "OnCaptureStart";
    public static string ON_TAP = "OnTap";
    public int NumOfCaptured;
    public Vector2 PosOfNew;
    public float SizeOfNew;
    public string TypeOfNew;
    public bool Special;

    public CaptureEvent(string aType_str, int numOfCaptured, Vector2 posOfNew, float sizeOfNew = 0f, bool special = false, string typeOfNew = null)
        : base(aType_str)
    {
        NumOfCaptured = numOfCaptured;
        PosOfNew = posOfNew;
        SizeOfNew = sizeOfNew;
        TypeOfNew = typeOfNew;
        Special = special;
    }

    public CaptureEvent(string aType_str)
        : base(aType_str)
    {

    }
}
