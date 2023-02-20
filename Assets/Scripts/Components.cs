using UnityEngine;
using XGameFramework.Event;

namespace XGameFramework.Components
{
    internal class Components : MonoBehaviour
    {
        private static EventManager m_eventComponent;
        private void Awake()
        {
            m_eventComponent = new EventManager();
        }

        public static EventManager eventComponent
        {
            get;
            private set;
        }
    }
}

