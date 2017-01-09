using System.Collections;
using com.rmc.projects.event_dispatcher;
using UnityEngine;

public class CreatureEvent : com.rmc.projects.event_dispatcher.Event
{

    public static string ON_SPECIAL_START = "OnSpecialStart";
    public static string ON_SPECIAL_COMPLETE = "OnSpecialComplete";

    public CreatureEvent(string aType_str)
        : base(aType_str)
    {

    }
}
