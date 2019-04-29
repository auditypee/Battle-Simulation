﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace States
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }

    public class StateMachine
    {
        private IState _currentState;

        public void ChangeState(IState newState)
        {
            if (_currentState != null)
                _currentState.Exit();

            _currentState = newState;
            _currentState.Enter();
        }

        public void Update()
        {
            _currentState.Execute();
        }
    }
}

