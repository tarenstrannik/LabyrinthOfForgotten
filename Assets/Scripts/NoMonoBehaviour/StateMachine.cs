using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine 
{
    private IState m_currentState;

    private class Transition
    {
        public Func<bool> Condition { get; }
        public IState TargetState { get; }

        public Transition(IState targetState, Func<bool> condition)
        {
            Condition = condition;
            TargetState = targetState;
        }
    }

    private Dictionary<Type, List<Transition>> m_allTransitionsWithFromDefinedState = new Dictionary<Type, List<Transition>>();

    private List<Transition> m_fromCurrentStateTransitions = new List<Transition>();
    private List<Transition> m_fromAnyStateTransitions = new List<Transition>();

    private static List<Transition> m_noTransitions = new List<Transition>(0);

    public void Process()
    {
        var transition = GetTransition();
        if (transition != null)
            SetState(transition.TargetState);

        m_currentState?.Process();
    }

    private Transition GetTransition()
    {
        foreach(var transition in m_fromAnyStateTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        foreach (var transition in m_fromCurrentStateTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        return null;
    }

    public void SetState(IState state)
    {
        if (state == m_currentState)
            return;

        m_currentState?.Exit();

        m_currentState = state;
        m_allTransitionsWithFromDefinedState.TryGetValue(m_currentState.GetType(), out m_fromCurrentStateTransitions);
        if (m_fromCurrentStateTransitions == null)
            m_fromCurrentStateTransitions = m_noTransitions;

        m_currentState.Enter();
    }

    public void AddFromDefinedStateTransition(IState fromState,IState toState, Func<bool> condition)
    {
        if (m_allTransitionsWithFromDefinedState.TryGetValue(fromState.GetType(), out var transitions) == false)
        {
            transitions= new List<Transition>();
            m_allTransitionsWithFromDefinedState[fromState.GetType()] = transitions;
        }


        transitions.Add(new Transition(toState, condition));
    }

    public void AddFromAnyStatetransition(IState toState, Func<bool> condition)
    {
        m_fromAnyStateTransitions.Add(new Transition(toState, condition));
    }
}
