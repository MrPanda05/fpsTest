using NUnit.Framework;
using System;
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
        public event Action OnStateEnter;
        public event Action OnStateExit;
        public virtual void Begin()
        {
            _parentStateMachine = transform.parent?.GetComponentInParent<HStateMachine>();
            _myStateMachine = GetComponent<HStateMachine>();
            _parentStateMachine.OnStateChangeTo += DisableSelf;
        }
        protected void DisableSelf(string state)
        {
            if (name == state) return;
            this.gameObject.SetActive(false);
            this.enabled = false;
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
