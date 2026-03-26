using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestGame.Commons.StateMachines
{
    public class HierarchicalState : MonoBehaviour, IState
    {
        [SerializeField]
        protected HStateMachine _parentStateMachine;
        [SerializeField]
        protected HStateMachine _myStateMachine;//It will be null if the gameojbect with this one does not have an hsm
        public virtual void Begin()
        {
            _parentStateMachine = transform.parent?.GetComponentInParent<HStateMachine>();
            _myStateMachine = GetComponent<HStateMachine>();
            //print($"My name is {name} parent state machine has name of { _parentStateMachine.name}");
        }
        public virtual void Enter()
        {
            print("Entering " + name);
            if (_myStateMachine != null)
            {
                // Ensure the sub-machine starts fresh when this state becomes active
                if (_myStateMachine.CurrentState == null && _myStateMachine._initialState != null)
                {
                    _myStateMachine.CurrentState = _myStateMachine._initialState;
                    _myStateMachine.CurrentState?.Enter();
                }
                else
                {
                    _myStateMachine.CurrentState?.Enter(); // re-enter if already set
                }
            }
        }
        public virtual void Exit()
        {
            print("Exiting " + name);
            if (_myStateMachine != null)
            {
                _myStateMachine.CurrentState?.Exit();
                _myStateMachine.ForceNullState(); // cleanly stops all sub-machines
            }
        }
        public virtual void Process() { }
        public virtual void FixProcess() { }
    }
}
