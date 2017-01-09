using System.Collections;
using com.rmc.projects.event_dispatcher;

public class GameEvent : Event
{

    public static string ON_LEVEL_COMPLETE = "OnLevelComplete";

    public GameEvent(string aType_str)
        : base(aType_str)
    {

    }
}