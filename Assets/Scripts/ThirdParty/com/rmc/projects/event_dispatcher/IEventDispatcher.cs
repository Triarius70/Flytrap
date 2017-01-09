/**
 * Copyright (C) 2005-2013 by Rivello Multimedia Consulting (RMC).                    
 * code [at] RivelloMultimediaConsulting [dot] com                                                  
 *                                                                      
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the      
 * "Software"), to deal in the Software without restriction, including  
 * without limitation the rights to use, copy, modify, merge, publish,  
 * distribute, sublicense, and#or sell copies of the Software, and to   
 * permit persons to whom the Software is furn
 * 
 * ished to do so, subject to
 * the following conditions:                                            
 *                                                                      
 * The above copyright notice and this permission notice shall be       
 * included in all copies or substantial portions of the Software.      
 *                                                                      
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,      
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF   
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR    
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.                                      
 */
// Marks the right margin of code *******************************************************************

//--------------------------------------
//  Imports
//--------------------------------------

//--------------------------------------
//  Namespace
//--------------------------------------
namespace com.rmc.projects.event_dispatcher
{
	
	//--------------------------------------
	//  Class
	//--------------------------------------
	/// <summary>
	/// IEventDispatcher
	/// </summary>
	public interface IEventDispatcher
	{
	
		//--------------------------------------
		//  Properties
		//--------------------------------------
		// GETTER / SETTER
		
		// PUBLIC
		
		//--------------------------------------
		//  Methods
		//--------------------------------------
		
		// PUBLIC
		/// <summary>
		/// Adds the event listener.
		/// </summary>
		/// <returns><c>true</c>, if event listener was added, <c>false</c> otherwise.</returns>
		/// <param name="aEventType_string">A event type_string.</param>
		/// <param name="aEventDelegate">A event delegate.</param>
	    bool addEventListener(string aEventType_string, EventDelegate aEventDelegate);

		/// <summary>
		/// Adds the event listener.
		/// </summary>
		/// <returns><c>true</c>, if event listener was added, <c>false</c> otherwise.</returns>
		/// <param name="aEventType_string">A event type_string.</param>
		/// <param name="aEventDelegate">A event delegate.</param>
		/// <param name="eventDispatcherAddMode">Event dispatcher add mode.</param>
		bool addEventListener(string aEventType_string, EventDelegate aEventDelegate, EventDispatcherAddMode eventDispatcherAddMode);


		/// <summary>
		/// Hases the event listener.
		/// </summary>
		/// <returns><c>true</c>, if event listener was hased, <c>false</c> otherwise.</returns>
		/// <param name="aEventType_string">A event type_string.</param>
		/// <param name="aEventDelegate">A event delegate.</param>
	    bool hasEventListener(string aEventType_string, EventDelegate aEventDelegate);

		/// <summary>
		/// Removes the event listener.
		/// </summary>
		/// <returns><c>true</c>, if event listener was removed, <c>false</c> otherwise.</returns>
		/// <param name="aEventType_string">A event type_string.</param>
		/// <param name="aEventDelegate">A event delegate.</param>
	    bool removeEventListener(string aEventType_string, EventDelegate aEventDelegate);

		/// <summary>
		/// Removes all event listeners.
		/// </summary>
		/// <returns><c>true</c>, if all event listeners was removed, <c>false</c> otherwise.</returns>
	    bool removeAllEventListeners();

		/// <summary>
		/// Dispatchs the event.
		/// </summary>
		/// <returns><c>true</c>, if event was dispatched, <c>false</c> otherwise.</returns>
		/// <param name="aIEvent">A I event.</param>
	    bool dispatchEvent(IEvent aIEvent);
		
		// STATIC

		//--------------------------------------
		//  Events
		//--------------------------------------
		

	}
}
