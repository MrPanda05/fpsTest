using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace TestGame.Commons.StateMachines
{
    /// <summary>
    /// Hierachical state machine
    /// 
    /// This state machine works on a set of rules, based on how unity works
    /// 
    /// 1-The root must always be a pure fsm, meaning it must not have an state component
    /// 2-The children can only be a pure fsm, a state or both, meaning having both components.
    /// 3-If it is a pure fsm, it will run concurrently to the currently selected active state of the parent
    /// 4-If it is just a state, it will be just like a normal state machine
    /// 5-If the child has both, it will act more as a state, meaning that it will not run concurrently, meaning the parent fsm has to select the hybrid state to be active
    /// </summary>
    public class HStateMachine : MonoBehaviour
    {
        [SerializeField]
        public HierarchicalState _initialState;
        public Dictionary<string, HierarchicalState> States { get; private set; }
        [field: SerializeField]
        public HierarchicalState CurrentState { get; set; }
        public event Action<string> OnStateChangeTo;
        public event Action<string, string> OnStateChangeFromTo;
        public event Action OnStateChange;
        [field: SerializeField]
        public bool IsRoot { get; private set; } = false;



        public HStateMachine parentStateMachine;//The parent of this gameobject state machin
        public List<HStateMachine> subStateMachines;//The children state machines of this gameobject
        public HierarchicalState myParentState;//The state that is from the parent of this game object
        public HierarchicalState myState;//The state that this object has alongside the fsm.
        public bool _isParentPureFSM = false;
        public bool _amIPureFSM = false;

        private void Awake()
        {
            States = new Dictionary<string, HierarchicalState>();
            subStateMachines = new List<HStateMachine>();
            myState = GetComponent<HierarchicalState>();
            var parentGameobject = transform.parent;
            parentStateMachine = parentGameobject.GetComponent<HStateMachine>();
            myParentState = parentGameobject.GetComponent<HierarchicalState>();
            _isParentPureFSM = myParentState == null && parentStateMachine != null;
            _amIPureFSM = myState == null;
            if (parentStateMachine == null) IsRoot = true;
            if(!IsRoot && myParentState == null && _amIPureFSM)
            {
                Debug.LogWarning("The parent is also a pure fsm, this can cause problems. " + name);
                print(myParentState?.name);
            }
            foreach (Transform child in transform)
            {
                var state = child.GetComponent<HierarchicalState>();
                var subFSM = child.GetComponent<HStateMachine>();
                //case 1 child has only state
                if (state != null && subFSM == null)
                {
                    States[state.name] = state;
                    state.Begin();
                }
                //case 2 child has only fsm
                if (subFSM != null && state == null)
                {
                    subStateMachines.Add(subFSM);
                }
                //case 3 child has both
                if (state != null && subFSM != null)
                {
                    States[state.name] = state;
                    state.Begin();
                    subStateMachines.Add(subFSM);
                }
            }
            foreach (var (name, state) in States)
            {
                state.enabled = false;
                state.gameObject.SetActive(false);
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
            if(myParentState != null)
            {
                print(name);
                myParentState.OnStateExit += DisableSelf;
                myParentState.OnStateEnter += EnableSelf;
                return;
                
            }
            if (parentStateMachine != null && !parentStateMachine.enabled) return;
            CurrentState = _initialState;
            CurrentState.enabled = true;
            CurrentState.gameObject.SetActive(true);
            CurrentState?.Enter();


        }
        private void OnDestroy()
        {
            if (myParentState != null)
            {
                myParentState.OnStateExit -= DisableSelf;
                myParentState.OnStateEnter -= EnableSelf;
            }
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
        private void EnableSelf()
        {
            print("My parentState is entering");
            enabled = true;
            gameObject.SetActive(true);
            CurrentState = _initialState;
            CurrentState.enabled = true;
            CurrentState.gameObject.SetActive(true);
            CurrentState?.Enter();
        }
        private void DisableSelf()
        {
            print("My parentState is exiting");
            enabled = false;
            gameObject.SetActive(false);
            CurrentState?.Exit();
        }
    }
}
