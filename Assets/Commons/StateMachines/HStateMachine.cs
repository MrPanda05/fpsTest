using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Commons.StateMachines
{
    /// <summary>
    /// Hierachical state machine, basicly the same script as normal state machine
    /// </summary>
    public class HStateMachine : MonoBehaviour
    {
        [SerializeField]
        public HierarchicalState _initialState;
        public Dictionary<string, HierarchicalState> States { get; private set; }
        [field: SerializeField]
        public HierarchicalState CurrentState { get; set; }
        public event Action OnStateChange;

        private void Awake()
        {
            States = new Dictionary<string, HierarchicalState>();
            foreach (Transform child in transform)
            {
                var state = child.GetComponent<HierarchicalState>();
                if (state != null)
                {
                    States[state.name] = state;
                    state.Begin();
                }
            }

            if (States.Count == 0)
            {
                Debug.LogWarning($"No direct child states found in HStateMachine '{name}'.");
                return;
            }
            if (_initialState == null)
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
            if (!States.ContainsKey(key))
            {
                Debug.LogWarning("State " + key + " not found in " + name);
                return;
            }
            if (CurrentState == States[key])
            {
                Debug.LogWarning("Already in state " + key + " in " + name);
                return;
            }

            CurrentState?.Exit();
            CurrentState = States[key];
            CurrentState?.Enter();
            OnStateChange?.Invoke();
        }
        public void ForceNullState()
        {
            CurrentState = null;
        }
    }
}
