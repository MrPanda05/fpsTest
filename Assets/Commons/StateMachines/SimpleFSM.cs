using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Commons.StateMachines
{
    /// <summary>
    /// Simple state machine to test complexity, create other type of fsm if this is too simple
    /// </summary>
    public class SimpleFSM : MonoBehaviour
    {
        /*
         * A simple FSM
         * 
         * 1 - has to have an reference to an initial state
         * 2 - Has get all states in children and store them in a dict
         * 3 - if has no state send warning
         * 4 - has to initialize each state, remembering to set the fsm to this
         * 5 - has to enter initial state
         * 6 - has to have events when a state changed
         * 7 - state will change based on the state name, calling exit and enter
         * 8 - the monobehaviou update and fix update will call the current state functions
         */
        [SerializeField]
        private SimpleState _initialState;
        public Dictionary<string, SimpleState> States { get; private set; }
        public SimpleState CurrentState { get; private set; }
        public event Action OnStateChange;

        private void Awake()
        {
            var childStates = GetComponentsInChildren<IState>();
            if(childStates.Length == 0)
            {
                Debug.LogWarning("No states found in children of " + name);
                return;
            }
            States = new Dictionary<string, SimpleState>();
            foreach (SimpleState item in childStates)
            {
                States[item.name] = item;
                item.Init(this);
                item.Begin();
            }
            if(_initialState == null)
            {
                Debug.LogWarning("No initial state set for " + name);
                return;
            }
            CurrentState = _initialState;
            CurrentState?.Enter();

        }
        private void Update()
        {
            CurrentState?.Process();
        }
        private void FixedUpdate()
        {
            CurrentState?.FixProcess();
        }
        public void TransitioToState(string key)
        {
            if(!States.ContainsKey(key))
            {
                Debug.LogWarning("State " + key + " not found in " + name);
                return;
            }
            if(CurrentState == States[key])
            {
                Debug.LogWarning("Already in state " + key + " in " + name);
                return;
            }

            CurrentState?.Exit();
            CurrentState = States[key];
            CurrentState?.Enter();
            OnStateChange?.Invoke();
        }
    }
}
