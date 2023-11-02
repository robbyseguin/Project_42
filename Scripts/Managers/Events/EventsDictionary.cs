using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Managers.Events
{
	public abstract class GeneralEventsDictionary 
	{ 
		public const int DEFAULT_EVENT = -1;
	}

	public class EventsDictionary<TCaller> : GeneralEventsDictionary
	{
		private Dictionary<int, UnityEvent<CallbackContext>> _listeners =
			new Dictionary<int, UnityEvent<CallbackContext>>();

		internal void Subscribe(int eventID, UnityAction<CallbackContext> listener)
		{
			UnityEvent<CallbackContext> myEvent;

			if (!_listeners.TryGetValue(eventID, out myEvent))
				_listeners.Add(eventID, myEvent = new UnityEvent<CallbackContext>());

			myEvent.AddListener(listener);
		}

		internal void Unsubscribe(int eventID, UnityAction<CallbackContext> listener)
		{
			UnityEvent<CallbackContext> myEvent;

			if (_listeners.TryGetValue(eventID, out myEvent))
				myEvent.RemoveListener(listener);
		}
		
		internal void Publish(TCaller caller, int eventID)
		{
			CallbackContext ctx = new CallbackContext(caller, eventID);
			UnityEvent<CallbackContext> myEvent;
			
			if (_listeners.TryGetValue(eventID, out myEvent)) 
				myEvent.Invoke(ctx);
			
			if(eventID != DEFAULT_EVENT && _listeners.TryGetValue(DEFAULT_EVENT, out myEvent)) 
				myEvent.Invoke(ctx);
		}

		internal void Publish<TValue>(TCaller caller, int eventID, TValue info)
		{
			CallbackContext ctx = new CallbackContext<TValue>(caller, eventID, info);
			UnityEvent<CallbackContext> myEvent;

			if (_listeners.TryGetValue(eventID, out myEvent))
				myEvent.Invoke(ctx);
			
			if(eventID != DEFAULT_EVENT && _listeners.TryGetValue(DEFAULT_EVENT, out myEvent)) 
				myEvent.Invoke(ctx);
		}


		public class CallbackContext
		{
			public TCaller Caller{ get;  }
			public int EventID { get;  }

			public CallbackContext(TCaller caller, int eventID)
			{
				Caller = caller;
				EventID = eventID;
			}

			public virtual TValue ReadValue<TValue>()
			{
#if UNITY_EDITOR
				Debug.LogWarning("Trying to read a non existing value, returning default value.");
#endif
				return default;
			}
		}

		public class CallbackContext<TInfo> : CallbackContext
		{
			private TInfo _info;

			public CallbackContext(TCaller caller, int eventID, TInfo info) : base(caller, eventID)
			{
				_info = info;
			}

			public override TValue ReadValue<TValue>()
			{
				return (TValue)Convert.ChangeType(_info, typeof(TValue));
			}
		}
	}
}