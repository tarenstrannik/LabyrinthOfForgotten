using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyHole : MonoBehaviour
{

    [Header("Events options")]

    [SerializeField] private UnityEvent m_activatedEvent;
    public UnityEvent OnActivated
    {
        get
        {
            return m_activatedEvent;
        }

    }
    [SerializeField] private UnityEvent m_deactivatedEvent;
    public UnityEvent OnDeactivated
    {
        get
        {
            return m_deactivatedEvent;
        }

    }
    public void Activate()
    {
        OnActivated.Invoke();
    }
    public void Dectivate()
    {
        OnDeactivated.Invoke();
    }
}
