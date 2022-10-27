using System;
using System.Collections.Generic;
namespace XGameFramework
{
    public static partial class ReferencePool
    {
        private static readonly Dictionary<Type, ReferenceCollection> s_ReferenceCollections = new Dictionary<Type, ReferenceCollection>();
        private static bool m_EnableStricCheck = false;

        public static bool EnableStricCheck
        {
            get
            {
                return m_EnableStricCheck;
            }
            set
            {
                m_EnableStricCheck = value;
            }
        }

        public static int Count
        {
            get
            {
                return s_ReferenceCollections.Count;
            }
        }

        public static ReferencePoolInfo[] GetAllReferencePoolInfo()
        {
            int index = 0;
            ReferencePoolInfo[] resules = null;
            lock (s_ReferenceCollections)
            {
                resules = new ReferencePoolInfo[s_ReferenceCollections.Count];
                foreach (KeyValuePair<Type, ReferenceCollection> referenceCollection in s_ReferenceCollections)
                {
                    resules[index++] = new ReferencePoolInfo(referenceCollection.Key, referenceCollection.Value.UnusedReferenceCount, referenceCollection.Value.UsingReferenceCount, referenceCollection.Value.AcquireReferenceCount, referenceCollection.Value.ReleaseReferenceCount, referenceCollection.Value.AddReferenceCount, referenceCollection.Value.RemoveReferenceCount);

                }
            }
            return resules;
        }

        public static void ClearAll()
        {
            lock (s_ReferenceCollections)
            {
                foreach (KeyValuePair<Type, ReferenceCollection> referenceCollection in s_ReferenceCollections)
                {
                    referenceCollection.Value.RemoveAll();
                }
                s_ReferenceCollections.Clear();
            }
        }

        public static T Acquire<T>() where T : class, IReference, new()
        {
            return GetReferenceCollection(typeof(T)).Acquire<T>();
        }

        public static IReference Acquire(Type referenceType)
        {
            return GetReferenceCollection(referenceType).Acquire();
        }

        public static void Release(IReference reference)
        {
            if (reference == null)
            {
                throw new GameFrameworkException("Reference is invalid.");
            }

            Type referenceType = reference.GetType();
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Release(reference);
        }

        public static void Add<T>(int count) where T : class, IReference, new()
        {
            GetReferenceCollection(typeof(T)).Add<T>(count);
        }

        public static void Add(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Add(count);
        }

        public static void Remove<T>(int count) where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).Remove(count);
        }

        public static void Remove(Type referenceType, int count)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).Remove(count);
        }

        public static void RemoveAll<T>() where T : class, IReference
        {
            GetReferenceCollection(typeof(T)).RemoveAll();
        }

        public static void RemoveAll(Type referenceType)
        {
            InternalCheckReferenceType(referenceType);
            GetReferenceCollection(referenceType).RemoveAll();
        }

        private static void InternalCheckReferenceType(Type referenceType)
        {
            if (!m_EnableStricCheck)
            {
                return;
            }
            if (referenceType == null)
            {
                throw new GameFrameworkException("Reference type is invalid.");
            }
            if (!referenceType.IsClass || referenceType.IsAbstract)
            {
                throw new GameFrameworkException("Reference type is not a non-abstract class type.");
            }
            if (!typeof(IReference).IsAssignableFrom(referenceType))
            {
                throw new GameFrameworkException(string.Format("Reference type '{0}' is invalid.", referenceType.FullName));
            }
        }

        private static ReferenceCollection GetReferenceCollection(Type referenceType)
        {
            if (referenceType == null)
            {
                throw new GameFrameworkException("ReferenceType is invalid.");
            }
            ReferenceCollection referenceCollection = null;
            lock (s_ReferenceCollections)
            {
                if (!s_ReferenceCollections.TryGetValue(referenceType, out referenceCollection))
                {
                    referenceCollection = new ReferenceCollection(referenceType);
                    s_ReferenceCollections.Add(referenceType, referenceCollection);
                }
            }
        }

    }
}