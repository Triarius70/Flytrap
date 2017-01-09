using UnityEngine;
using System.Collections;
using com.rmc.projects.event_dispatcher;

public class EventDispatcherBase : MonoBehaviour {
    public EventDispatcher eventDispatcher;

    public EventDispatcherBase()
    {
        eventDispatcher = new EventDispatcher(this);
    }

    ~EventDispatcherBase()
	{

	}

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    public virtual void OnDestroy()
    {
        //	CLEANUP MEMORY
        eventDispatcher.removeAllEventListeners();
        eventDispatcher = null;
    }
}
