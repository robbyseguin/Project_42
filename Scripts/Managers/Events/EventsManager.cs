using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Managers.Events
{
	public static class EventsManager
	{
		private static Dictionary<Type, GeneralEventsDictionary> _eventsDictionaries = 
			new Dictionary<Type, GeneralEventsDictionary>();

		public static void Subscribe<TPublisherType>(
			UnityAction<EventsDictionary<TPublisherType>.CallbackContext> listener,
			int eventID = GeneralEventsDictionary.DEFAULT_EVENT
		)
		{
			Type key = typeof(TPublisherType);
			GeneralEventsDictionary myEvent;

			if (!_eventsDictionaries.TryGetValue(key, out myEvent))
				_eventsDictionaries.Add(key, myEvent = new EventsDictionary<TPublisherType>());

			(myEvent as EventsDictionary<TPublisherType>)?.Subscribe(eventID, listener);
		}

		public static void Unsubscribe<TPublisherType>(
			UnityAction<EventsDictionary<TPublisherType>.CallbackContext> listener,
			int eventID = GeneralEventsDictionary.DEFAULT_EVENT
		)
		{
			Type key = typeof(TPublisherType);
			GeneralEventsDictionary myEvent;

			if (_eventsDictionaries.TryGetValue(key, out myEvent))
				(myEvent as EventsDictionary<TPublisherType>)?.Unsubscribe(eventID, listener);
		}

		public static void Publish<TProviderType>(
			this TProviderType publisher, 
			int eventID = GeneralEventsDictionary.DEFAULT_EVENT
			)
		{
			Type key = typeof(TProviderType);
			GeneralEventsDictionary myEvent;

			if (!_eventsDictionaries.TryGetValue(key, out myEvent)) return;
			
			(myEvent as EventsDictionary<TProviderType>)?.Publish(publisher, eventID);
		}

		public static void Publish<TProviderType, TInfoType>(
			this TProviderType publisher, 
			TInfoType info, 
			int eventID = GeneralEventsDictionary.DEFAULT_EVENT
			)
		{
			Type key = typeof(TProviderType);
			GeneralEventsDictionary myEvent;

			if (!_eventsDictionaries.TryGetValue(key, out myEvent)) return;
			
			(myEvent as EventsDictionary<TProviderType>)?.Publish(publisher, eventID, info);
		}
	}
}