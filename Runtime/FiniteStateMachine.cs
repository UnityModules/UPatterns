using System.Collections.Generic;

namespace UPatterns
{
    public class UFiniteStateMachine<T>
    {
        protected Dictionary<T, UState<T>> states = new ();

        protected UState<T> currentState;
        public UState<T> CurrentState => currentState;
        public T CurrentStateID
        {
            get
            {
                foreach (var state in states)
                    if (state.Value == currentState)
                        return state.Key;

                return default(T);
            }
        }
        public bool HasAState => currentState != null;

        public void Add(UState<T> state) => states.Add(state.ID, state);
        public void Add(T stateID, UState<T> state) => states.Add(stateID, state);

        public UState<T> GetState(T stateID) =>
            states.ContainsKey(stateID) ? states[stateID] : null;

        public void SetCurrentState(T stateID) =>
            SetCurrentState(states[stateID]);

        public UState<T> GetCurrentState => currentState;

        public void SetCurrentState(UState<T> state)
        {
            ExitCurrentState();
            EnterState(state);
        }

        public void ExitCurrentState()
        {
            if (currentState != null)
                currentState.Exit();

            currentState = null;
        }

        public void EnterState(T stateID) =>
            EnterState(states[stateID]);
        public void EnterState(UState<T> state)
        {
            if (currentState != null)
            {
                SetCurrentState(state);
                return;
            }

            if (currentState == state)
                return;

            currentState = state;

            if (currentState != null)
                currentState.Enter();
        }

        public void Update()
        {
            if (currentState != null)
                currentState.Updates();
        }
        public void FixedUpdate()
        {
            if (currentState != null)
                currentState.FixedUpdates();
        }
    }
}