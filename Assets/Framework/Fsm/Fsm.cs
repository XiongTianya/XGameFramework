using System;
using System.Collections.Generic;
namespace XGameFramework.Fsm
{
    internal sealed class Fsm<T> : FsmBase, IFsm<T> where T : class
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
            fsm.CurrentStateName = name;
            fsm.m_Owner = owner;
            fsm.m_IsDestroyed = false;
            foreach (FsmState<T> state in states)
            {
                if (state == null)
                {
                    Debug.LogError("FSM states is invalid.");
                }
                Type stateType = state.GetType();
                if (fsm.m_States.ContainsKey(stateType))
                {
                    Debug.LogError(Utility.Text.Format("FSM '{0}' state '{1}' is already exist.", new TypeNamePair(typeof(T), name), stateType.FullName));
                }
                fsm.m_States.Add(stateType, state);
                state.OnInit(fsm);
            }
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
                    throw new GameFrameworkException(Utility.Text.Format("FSM '{0}' state '{1}' is already exist.", new TypeNamePair(typeof(T), name), stateType.FullName));
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
        }

    }
}