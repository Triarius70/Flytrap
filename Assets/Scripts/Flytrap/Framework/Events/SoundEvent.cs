using System.Collections;
using com.rmc.projects.event_dispatcher;
using UnityEngine;

public class SoundEvent : com.rmc.projects.event_dispatcher.Event
{

    public static string ON_INTRO_START = "OnIntroStart";
    public static string ON_LEVEL_START = "OnLevelStart";
    public static string ON_LEVEL_COMPLETE = "OnLevelComplete";
    public static string ON_CAPTURE_START = "OnCaptureStart";
    public static string ON_CAPTURE_COMPLETE = "OnCaptureComplete";
    public static string ON_SPECIAL_COMPLETE = "OnSpecialComplete";
    public static string ON_SPECIAL_START = "OnSpecialStart";
    public static string ON_GAME_OVER = "OnGameOver";
    public static string ON_RESTART = "OnRestart";
    public int NumOfCaptured;
    public bool Special;

    public SoundEvent(string aType_str)
        : base(aType_str)
    {

    }

    public SoundEvent(string aType_str, int numOfCaptured, bool special)
        : base(aType_str)
    {
        NumOfCaptured = numOfCaptured;
        Special = special;
    }
}
