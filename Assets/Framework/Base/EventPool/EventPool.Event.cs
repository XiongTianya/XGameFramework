/*
 * @Author: xiongtianya 
 * @Date: 2023-01-12 17:47:08 
 * @Last Modified by: xiongtianya
 * @Last Modified time: 2023-01-12 17:57:22
 */
namespace XGameFramework
{
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private sealed class Event : IReference
        {
            private object m_Sender;
            private T m_EventArgs;
            public Event()
            {
                m_Sender = null;
                m_EventArgs = null;
            }

            public object Sender
            {
                get
                {
                    return m_Sender;

                }
            }

            public T EventArgs
            {
                get
                {
                    return m_EventArgs;
                }
            }

            public static Event Create(object sender, T e)
            {
                Event eventNode = ReferencePool.Acquire<Event>();
                eventNode.m_Sender = sender;
                eventNode.m_EventArgs = e;
                return eventNode;
            }

            public void Clear()
            {
                m_Sender = null;
                m_EventArgs = null;
            }
        }
    }
}