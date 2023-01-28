/*
 * @Author: xiongtianya 
 * @Date: 2023-01-12 16:54:29 
 * @Last Modified by:   xiongtianya 
 * @Last Modified time: 2023-01-12 16:54:29 
 */
using System;
using System.Collections.Generic;
namespace XGameFramework.Fsm
{
    internal sealed class Fsm<T> : FsmBase, IReference, IFsm<T> where T : class
    {
        private T m_Owner;
        private readonly Dictionary<Type, FsmState<T>> m_States;
        private Dictionary<string, Variable> m_Datas;
        private FsmState<T> m_CurrentState;
        private float m_CurrentStateTime;
        private bool m_IsDestroyed;

        public Fsm()
        {
            m_Owner = null;
            m_States = new Dictionary<Type, FsmState<T>>();
            m_Datas = null;
            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

        public T Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public override Type OwnerType
        {
            get
            {
                return typeof(T);
            }
        }

        public override int FsmStateCount
        {
            get
            {
                return m_States.Count;
            }
        }

        public override bool IsRunning
        {
            get
            {
                return m_CurrentState != null;
            }
        }

        public override bool IsDestroyed
        {
            get
            {
                return m_IsDestroyed;
            }
        }

        public FsmState<T> CurrentState
        {
            get
            {
                return m_CurrentState;
            }
        }

        public override string CurrentStateName
        {
            get
            {
                return m_CurrentState != null ? m_CurrentState.GetType().FullName : null;
            }
        }

        public override float CurrentStateTime
        {
            get
            {
                return m_CurrentStateTime;
            }
        }


        public static Fsm<T> Create(string name, T owner, params FsmState<T>[] states)
        {
            if (owner == null)
            {
                Console.WriteLine("FSM owner is invalid.");
            }

            if (states == null || states.Length < 1)
            {
                Console.WriteLine("FSM states is invalid.");
            }

            Fsm<T> fsm = new Fsm<T>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new GameFrameworkException("FSM states is invalid.");
                }
                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new GameFrameworkException(String.Format("FSM '{0}' state '{1}' is already exist.", new TypeNamePair(typeof(T), name), stateType.FullName));
                }
                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }
            return fsm;
        }

        public static Fsm<T> Create(string name, T owner, List<FsmState<T>> states)
        {
            if (owner == null)
            {
                throw new GameFrameworkException("FSM owner is invalid.");
            }

            if (states == null || states.Count < 1)
            {
                throw new GameFrameworkException("FSM states is invalid.");
            }

            Fsm<T> fsm = ReferencePool.Acquire<Fsm<T>>();
            fsm.Name = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    throw new GameFrameworkException("FSM states is invalid.");
                }

                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    throw new GameFrameworkException(string.Format("FSM '{0}' state '{1}' is already exist.", new TypeNamePair(typeof(T), name), stateType.FullName));
                }

                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }

            return fsm;
        }

        public void Clear()
        {
            if (m_CurrentState != null)
            {
                m_CurrentState.OnLeave(this, true);
            }
            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                state.Value.OnDestroy(this);
            }
            Name = null;
            m_Owner = null;
            m_States.Clear();

            if (m_Datas != null)
            {
                foreach (KeyValuePair<string, Variable> data in m_Datas)
                {
                    if (data.Value == null)
                    {
                        continue;
                    }
                    ReferencePool.Release(data.Value);
                }
                m_Datas.Clear();
            }
            m_CurrentState = null;
            m_CurrentStateTime = 0f;
            m_IsDestroyed = true;
        }

        public void Start<TState>() where TState : FsmState<T>
        {
            if (IsRunning)
            {
                throw new GameFrameworkException("FSM is running, can not start again.");
            }

            FsmState<T> state = GetState<TState>();
            if (state == null)
            {
                throw new GameFrameworkException(String.Format("FSM '{0}' can not start state '{1}' which is not exist.", new TypeNamePair(typeof(T), Name), typeof(TState).FullName));
            }
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        public void Start(Type stateType)
        {
            if (IsRunning)
            {
                throw new GameFrameworkException("FSM is running, can not start again.");
            }

            if (stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(String.Format("State type '{0}' is invalid.", stateType.FullName));
            }
            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new GameFrameworkException(String.Format("FSM '{0}' can not start state '{1}' which is not exist.", new TypeNamePair(typeof(T), Name), stateType.FullName));
            }
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }

        public bool HasState<TState>() where TState : FsmState<T>
        {
            return m_States.ContainsKey(typeof(TState));
        }

        public bool HasState(Type stateType)
        {
            if (stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }
            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(String.Format("State type '{0}' is invalid.", stateType.FullName));
            }
            return m_States.ContainsKey(stateType);
        }


        public TState GetState<TState>() where TState : FsmState<T>
        {
            FsmState<T> state = null;
            if (m_States.TryGetValue(typeof(TState), out state))
            {
                return (TState)state;
            }
            return null;
        }

        public FsmState<T> GetState(Type stateType)
        {
            if (stateType == null)
            {
                throw new GameFrameworkException("State type is invalid.");
            }

            if (!typeof(FsmState<T>).IsAssignableFrom(stateType))
            {
                throw new GameFrameworkException(String.Format("State type '{0}' is invalid.", stateType.FullName));
            }

            FsmState<T> state = null;
            if (m_States.TryGetValue(stateType, out state))
            {
                return state;
            }

            return null;
        }

        public FsmState<T>[] GetAllStates()
        {
            int index = 0;
            FsmState<T>[] results = new FsmState<T>[m_States.Count];
            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                results[index++] = state.Value;
            }
            return results;
            //为什么不直接Values()
            //return m_States.Values();
        }

        public void GetAllStates(List<FsmState<T>> results)
        {
            if (results == null)
            {
                throw new GameFrameworkException("Results is invalid.");
            }

            results.Clear();
            foreach (KeyValuePair<Type, FsmState<T>> state in m_States)
            {
                results.Add(state.Value);
            }
        }

        public bool HasData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                return false;
            }

            return m_Datas.ContainsKey(name);
        }

        public TData GetData<TData>(string name) where TData : Variable
        {
            return (TData)GetData(name);
        }

        public Variable GetData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                return null;
            }

            Variable data = null;
            if (m_Datas.TryGetValue(name, out data))
            {
                return data;
            }

            return null;
        }

        public void SetData<TData>(string name, TData data) where TData : Variable
        {
            SetData(name, (Variable)data);
        }

        public void SetData(string name, Variable data)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                m_Datas = new Dictionary<string, Variable>(StringComparer.Ordinal);
            }

            Variable oldData = GetData(name);
            if (oldData != null)
            {
                ReferencePool.Release(oldData);
            }

            m_Datas[name] = data;
        }

        public bool RemoveData(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new GameFrameworkException("Data name is invalid.");
            }

            if (m_Datas == null)
            {
                return false;
            }

            Variable oldData = GetData(name);
            if (oldData != null)
            {
                ReferencePool.Release(oldData);
            }

            return m_Datas.Remove(name);
        }

        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (m_CurrentState == null)
            {
                return;
            }

            m_CurrentStateTime += elapseSeconds;
            m_CurrentState.OnUpdate(this, elapseSeconds, realElapseSeconds);
        }

        internal override void Shutdown()
        {
            ReferencePool.Release(this);
        }

        internal void ChangeState<TState>() where TState : FsmState<T>
        {
            ChangeState(typeof(TState));
        }

        internal void ChangeState(Type stateType)
        {
            if (m_CurrentState == null)
            {
                throw new GameFrameworkException("Current state is invalid.");
            }

            FsmState<T> state = GetState(stateType);
            if (state == null)
            {
                throw new GameFrameworkException(String.Format("FSM '{0}' can not change state to '{1}' which is not exist.", new TypeNamePair(typeof(T), Name), stateType.FullName));
            }

            m_CurrentState.OnLeave(this, false);
            m_CurrentStateTime = 0f;
            m_CurrentState = state;
            m_CurrentState.OnEnter(this);
        }
    }
}