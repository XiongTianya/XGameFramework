using System;
namespace XGameFramework.Fsm
{
    public abstract class FsmState<T> where T : class
    {
        public FsmState()
        {

        }

        protected internal virtual void OnInit(IFsm<T> fsm)
        {

        }

        protected internal virtual void OnEnter(IFsm<T> fsm)
        {

        }

        protected internal virtual void OnUpdate(IFsm<T> fsm, float elapseSeconds, float realElapseSeconds)
        {

        }

        protected internal virtual void OnLeave(IFsm<T> fsm, bool isShutdown)
        {

        }

        protected internal virtual void OnDestroy(IFsm<T> fsm)
        {

        }

        protected void ChangeState<TState>(IFsm<T> fsm) where TState : FsmState<T>
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                //throw new GameFrameworkException("FSM is invalid.");
            }

        }

        protected void ChangeState(IFsm<T> fsm, Type stateType)
        {
            Fsm<T> fsmImplement = (Fsm<T>)fsm;
            if (fsmImplement == null)
            {
                //throw new GameFrameworkException("FSM is invalid.");
            }
        }
    }
}