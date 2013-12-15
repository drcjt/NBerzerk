using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBerzerk
{
    public interface IState
    {
        string StateName { get; }
        void EnterState();
        void LeaveState();
    };

    public class StateManager<T> where T : IState
    {
        IDictionary<string, T> states = new Dictionary<string, T>();

        T currentState;

        public T CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public StateManager()
        {
        }

        public void AddState(T state, bool defaultState = false)
        {
            states.Add(state.StateName, state);
            if (defaultState)
            {
                CurrentState = state;
            }
        }

        public void SwitchState(string stateName)
        {
            CurrentState.LeaveState();
            CurrentState = states[stateName];
            CurrentState.EnterState();
        }
    }
}
