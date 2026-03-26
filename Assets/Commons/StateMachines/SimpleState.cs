using UnityEngine;

namespace TestGame.Commons.StateMachines
{
    /// <summary>
    /// THis should only be used with the simple fsm
    /// </summary>
    public abstract class SimpleState : MonoBehaviour, IState
    {
        /*
         * A simple state
         * Has to have an init, for initilization, it should only be executed once
         * an start also for initilization
         * An enter and exit
         * Update and a physics update
         * Also a reference to its own fsm
         */

        protected SimpleFSM _FSM;
        public void Init(SimpleFSM fsm)
        {
            _FSM = fsm;
        }
        public virtual void Begin() { }
        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Process() { }
        public virtual void FixProcess() { }
    }
}
