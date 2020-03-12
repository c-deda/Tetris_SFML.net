﻿using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class StateManager
    {
        private Stack<State> gameStates;
        public State newActiveState { get; set; }
        public bool isAdding { get; set; }
        public bool isRemoving { get; set; }
        public bool isReplacing { get; set; }

        public StateManager()
        {
            gameStates = new Stack<State>();
            isAdding = false;
            isRemoving = false;
            isReplacing = false;
        }

        public void addState(State newState, bool isReplacing)
        {
            this.isAdding = true;
            this.isReplacing = isReplacing;
            this.newActiveState = newState;

        }

        public void removeState(State newState, bool isReplacing)
        {
            this.isRemoving = true;
            this.isReplacing = isReplacing;
            this.newActiveState = newState;
        }

        public State GetActiveState()
        {
            return gameStates.Peek();
        }

        public void ProcessStateChanges(ref GameData data)
        {
            // Removing States
            while (isRemoving && gameStates.Any())
            {
                gameStates.Pop();

                if (GetActiveState().ToString() == newActiveState.ToString())
                {
                    if (isReplacing)
                    {
                        gameStates.Pop();
                        gameStates.Push(newActiveState);
                        newActiveState.Init(ref data);
                        isReplacing = false;
                    }

                    isRemoving = false;
                    newActiveState = null;

                    if (!gameStates.Any())
                    {
                        data.window.Close();
                    }
                }
            }
            // Adding States
            if (isAdding)
            {
                if (gameStates.Any())
                {
                    if (isReplacing)
                    {
                        gameStates.Pop();
                        isReplacing = false;
                    }
                }

                if (newActiveState != null)
                {
                    gameStates.Push(newActiveState);
                    newActiveState.Init(ref data);
                }

                isAdding = false;
            }
        }
    }
}