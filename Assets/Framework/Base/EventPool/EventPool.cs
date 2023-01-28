/*
 * @Author: xiongtianya 
 * @Date: 2023-01-13 11:42:27 
 * @Last Modified by: xiongtianya
 * @Last Modified time: 2023-01-13 17:22:01
 */

using System;
using System.Collections.Generic;
namespace XGameFramework
{
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private readonly GameFrameworkMultiDictionary<int, EventHandler<T>> m_EventHandlers;
        private readonly Queue<Event> m_Events;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> m_CachedNodes;
        private readonly Dictionary<object, LinkedListNode<EventHandler<T>>> m_TempNodes;
        private readonly EventPoolMode m_EventPoolMode;
        private EventHandler<T> m_DefaultHandler;

        public EventPool(EventPoolMode mode)
        {
            m_EventHandlers = new GameFrameworkMultiDictionary<int, EventHandler<T>>();
            m_Events = new Queue<Event>();
            m_CachedNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            m_TempNodes = new Dictionary<object, LinkedListNode<EventHandler<T>>>();
            m_EventPoolMode = mode;
            m_DefaultHandler = null;
        }

        public int EventHandlerCount
        {
            get
            {
                return m_EventHandlers.count;
            }
        }

        public int EventCount
        {
            get
            {
                return m_Events.Count;
            }
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            lock (m_Events)
            {
                while (m_Events.Count > 0)
                {
                    Event eventNode = m_Events.Dequeue();

                }
            }
        }

        public void Shutdown()
        {
            Clear();
            m_EventHandlers.Clear();
            m_CachedNodes.Clear();
            m_TempNodes.Clear();
            m_DefaultHandler = null;
        }

        public void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
        }

        public int Count(int id)
        {
            GameFrameworkLinkedListRange<EventHandler<T>> range = default(GameFrameworkLinkedListRange<EventHandler<T>>);
            if (m_EventHandlers.TryGetValue(id, out range))
            {
                return range.Count;
            }
            return 0;
        }

        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }
            return m_EventHandlers.Contains(id, handler);
        }

        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }
            if (!m_EventHandlers.Contains(id))
            {
                m_EventHandlers.Add(id, handler);
            }

        }
    }
}