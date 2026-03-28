using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestGame.Commons.StateMachines
{
    /// <summary>
    /// Base class for the state machine
    /// 
    /// IMPORTANT: always call base class, or remember to invoke the events enter, exit and specially begin, otherwise it breaks the state machine.
    /// 
    /// This state could be inherit, for example for a player state, then re-inherint for player to avoid repetition, but it work as is.
    /// </summary>
    public class HierarchicalState : MonoBehaviour, IState
    {
        [SerializeField]
        protected HStateMachine _parentStateMachine;
        [SerializeField]
        protected HStateMachine _myStateMachine;
        public event Action OnStateEnter;
        public event Action OnStateExit;
        public virtual void Begin()
        {
            _parentStateMachine = transform.parent != null ? transform.parent.GetComponent<HStateMachine>() : null;
            _myStateMachine = GetComponent<HStateMachine>();
        }
        public virtual void Enter()
        {
            print("Entering " + name);
            OnStateEnter?.Invoke();
        }
        public virtual void Exit()
        {
            print("Exiting " + name);
            OnStateExit?.Invoke();
        }
        public virtual void Process() { }
        public virtual void FixProcess() { }
    }
}
