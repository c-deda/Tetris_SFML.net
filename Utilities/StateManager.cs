using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class StateManager
    {
        private Stack<State> _gameStates;
        public State NewActiveState { get; private set; }
        public bool IsAdding { get; private set; }
        public bool IsRemoving { get; private set; }
        public bool IsReplacing { get; private set; }

        public StateManager()
        {
            _gameStates = new Stack<State>();
            IsAdding = false;
            IsRemoving = false;
            IsReplacing = false;
        }

        public void AddState(State newState, bool isReplacing)
        {
            IsAdding = true;
            IsReplacing = isReplacing;
            NewActiveState = newState;

        }

        public void RemoveState()
        {
            IsRemoving = true;
        }

        public State GetActiveState()
        {
            return _gameStates.Peek();
        }

        public void ProcessStateChanges(GameData data)
        {
            // Removing States
            while (IsRemoving && _gameStates.Any())
            {
                _gameStates.Pop();

                IsRemoving = false;

                if (!_gameStates.Any())
                {
                    data.Window.Close();
                }
            }
            // Adding States
            if (IsAdding)
            {
                if (_gameStates.Any())
                {
                    if (IsReplacing)
                    {
                        _gameStates.Pop();
                        IsReplacing = false;
                    }
                }

                if (NewActiveState != null)
                {
                    _gameStates.Push(NewActiveState);
                    NewActiveState.Init(data);
                }

                IsAdding = false;
            }
        }
    }
}
