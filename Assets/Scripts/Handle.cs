using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Handle : MonoBehaviour, IHaveMinMax, IMoveLinear
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

    [SerializeField] private UnityEvent<float> m_positionChangeBeginEvent;
    public UnityEvent<float> OnPositionChangeBegin
    {
        get
        {
            return m_positionChangeBeginEvent;
        }

    }
    [SerializeField] private UnityEvent m_positionChangingEndEvent;
    public UnityEvent OnPositionChangeEnd
    {
        get
        {
            return m_positionChangingEndEvent;
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
    public UnityEvent OnMinLeftEvent { get; }


    public void Activate()
    {
        OnMaxReachedEvent.Invoke();
    }
    public void Dectivate()
    {
        OnMaxLeftEvent.Invoke();
    }
}
