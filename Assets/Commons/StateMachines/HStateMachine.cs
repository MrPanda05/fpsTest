using System;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Commons.StateMachines
{
    /// <summary>
    /// Hierachical state machine
    /// 
    /// This state machine works on a set of rules, based on how unity works
    /// 
    /// //Cases
    /// 1 - A game obj with only HStateMachine component is a pure state machine
    /// 2 - A game obj with only HierarchicalState component is a pure state
    /// 3 - A game obj with both HStateMachine and HierarchicalState is a hybrid
    /// 
    /// The root game obj must always be a pure fsm
    /// The children of this root, can be either of the three cases
    /// 
    /// This state machine works like this.
    /// 
    /// If a child is a pure fsm, it will run concurrently to any state that the parent state machine is in.
    /// If a child is a hybrid, it will only be active when the component state that is posseses is entered.
    /// If it is a pure state, it will act as expected of a normal state machine.
    /// 
    /// Note: A pure state must not have a child of pure state machine, if this happen, then it means that the pure state must be a hybrid.
    /// 
    /// Possible improvements, a previous state variable, as it currently re-enters on the initial state
    /// </summary>
    public class HStateMachine : MonoBehaviour
    {
        [SerializeField]
        private HierarchicalState _initialState;
        public Dictionary<string, HierarchicalState> States { get; private set; }
        [field: SerializeField]
        public HierarchicalState CurrentState { get; private set; }
        public event Action<string> OnStateChangeTo;
        public event Action<string, string> OnStateChangeFromTo;
        public event Action OnStateChange;
        public HierarchicalState MyState { get; private set; }
        public HierarchicalState ParentState { get; private set; }
        public bool ImPure { get; private set; }
        //private List<HStateMachine> _concurrentFSMs = new List<HStateMachine>();

        private void Awake()
        {
            if (_initialState == null)
            {
                Debug.LogWarning("No initial state set for " + name);
                return;
            }
            States = new Dictionary<string, HierarchicalState>();
            //_concurrentFSMs = new List<HStateMachine>();
            GetRelationShipWithSelf();
            ParentState = transform.parent != null ? transform.parent.GetComponent<HierarchicalState>() : null;
            foreach (Transform child in transform)
            {
                var state = child.GetComponent<HierarchicalState>();
                if (state != null)
                {
                    States[state.name] = state;
                    state.Begin();
                    state.enabled = false;
                }
            }
            if (States.Count == 0)
            {
                Debug.LogWarning($"No direct child states found in HStateMachine '{name}'.");
                return;
            }
        }
        private void Start()
        {
            if(!ImPure)
            {
                MyState.OnStateEnter += InitializeFSM;
                MyState.OnStateExit += DisableFSM;
                return;
            }
            if (ParentState == null)
            {
                InitializeFSM();
            }
            else
            {
                ParentState.OnStateEnter += InitializeFSM;
                ParentState.OnStateExit += DisableFSM;
            }
        }
        private void GetRelationShipWithSelf()
        {
            MyState = GetComponent<HierarchicalState>();
            ImPure = MyState == null;
        }
        public void InitializeFSM()
        {
            CurrentState = _initialState;
            CurrentState.enabled = true;
            enabled = true;
            if(CurrentState != null)
            {
                CurrentState?.Enter();
            }
        }
        public void DisableFSM()
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            CurrentState.enabled = false;
            enabled = false;
        }
        private void OnDestroy()
        {
            if (MyState != null)
            {
                MyState.OnStateEnter -= InitializeFSM;
                MyState.OnStateExit -= DisableFSM;
            }
            if(ParentState != null)
            {
                ParentState.OnStateEnter -= InitializeFSM;
                ParentState.OnStateExit -= DisableFSM;
            }
        }
        private void Update()
        {
            if(CurrentState != null)
            {
                CurrentState.Process();
            }
        }
        private void FixedUpdate()
        {
            if (CurrentState != null) 
            { 
                CurrentState.FixProcess();
            }
        }
        public void TransitionTo(string key)
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
            if(CurrentState == null)
            {
                Debug.LogWarning("Current state is null in " + name);
                return;
            }
            string oldState = CurrentState.name;
            string newState = key;
            CurrentState.Exit();
            CurrentState.enabled = false;
            CurrentState = States[key];
            CurrentState.enabled = true;
            CurrentState.Enter();
            OnStateChange?.Invoke();
            OnStateChangeTo?.Invoke(key);
            OnStateChangeFromTo?.Invoke(oldState, newState);
        }
        public void ForceNullState()
        {
            CurrentState = null;
        }
    }
}
