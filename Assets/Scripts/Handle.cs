using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Handle : MonoBehaviour, IHaveMinMax
{
    // Events
    [Header("Events options")]

    [SerializeField] private UnityEvent m_activatedEvent;
    public UnityEvent OnMaxReachedEvent
    {
        get
        {
            return m_activatedEvent;
        }

    }
    [SerializeField] private UnityEvent m_deactivatedEvent;
    public UnityEvent OnMaxLeftEvent
    {
        get
        {
            return m_deactivatedEvent;
        }

    }


    [SerializeField] private UnityEvent m_startPositionReachedEvent;
    public UnityEvent OnMinReachedEvent
    {
        get
        {
            return m_startPositionReachedEvent;
        }
    }
    [SerializeField] private UnityEvent m_startPositionLeftEvent;
    public UnityEvent OnMinLeftEvent
    {
        get
        {
            return m_startPositionLeftEvent;
        }
    }

    public void Activate()
    {
        OnMaxReachedEvent.Invoke();
    }
    public void Dectivate()
    {
        OnMaxLeftEvent.Invoke();
    }
}
