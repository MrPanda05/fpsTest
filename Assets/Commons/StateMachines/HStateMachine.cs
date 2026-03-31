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
    /// Note: A pure state should not have a children, as it will not work, if the state needs have children, make them hybrid, this could be a possible issue in the future
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
        public HStateMachine ParentFSM { get; private set; }
        [field: SerializeField]
        public bool IsPure { get; private set; }
        public Action OnInitialize;
        public Action OnDisableFSM;
        private List<HStateMachine> _subStateMachines = new List<HStateMachine>();

        private void Awake()
        {
            if (_initialState == null)
            {
                Debug.LogWarning("No initial state set for " + name);
                return;
            }
            States = new Dictionary<string, HierarchicalState>();
            _subStateMachines = new List<HStateMachine>();
            GetRelationShipWithSelf();
            ParentState = transform.parent != null ? transform.parent.GetComponent<HierarchicalState>() : null;
            ParentFSM = transform.parent != null ? transform.parent.GetComponent<HStateMachine>() : null;
            foreach (Transform child in transform)
            {
                var state = child.GetComponent<HierarchicalState>();
                var fsm = child.GetComponent<HStateMachine>();
                if (state != null)
                {
                    States[state.name] = state;
                    state.Begin();
                }
                if (fsm != null)
                {
                    fsm.enabled = false;
                    _subStateMachines.Add(fsm);
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
            if (ParentState != null && ParentFSM != null) return;
            InitializeHierarchy();
            
        }
        private void GetRelationShipWithSelf()
        {
            MyState = GetComponent<HierarchicalState>();
            IsPure = MyState == null;
        }
        public void InitializeFSM()
        {
            enabled = true;
            CurrentState = _initialState;
            if (CurrentState != null)
            {
                CurrentState.Enter();
            }
            OnInitialize?.Invoke();
        }
        public void InitializeHierarchy()
        {
            if (ParentFSM != null)
            {
                if (MyState != null)
                {
                    MyState.OnStateEnter += InitializeFSM;
                    MyState.OnStateExit += DisableFSM;
                }
                else
                {
                    ParentFSM.OnInitialize += InitializeFSM;
                    ParentFSM.OnDisableFSM += DisableFSM;
                }
            }
            foreach (var fsm in _subStateMachines)
            {
                fsm.InitializeHierarchy();
            }
            if (ParentFSM == null)
            {
                InitializeFSM();
            }
        }
        public void DisableFSM()
        {
            if (CurrentState != null)
            {
                CurrentState.Exit();
            }
            OnDisableFSM?.Invoke();
            enabled = false;
        }
        private void OnDestroy()
        {
            if (ParentFSM != null)
            {
                if (MyState != null)
                {
                    MyState.OnStateEnter -= InitializeFSM;
                    MyState.OnStateExit -= DisableFSM;
                }
                else
                {
                    ParentFSM.OnInitialize -= InitializeFSM;
                    ParentFSM.OnDisableFSM -= DisableFSM;
                }
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
            //CurrentState.enabled = false;
            CurrentState = States[key];
            //CurrentState.enabled = true;
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