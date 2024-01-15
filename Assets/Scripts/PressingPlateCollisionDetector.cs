using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingPlateCollisionDetector : MonoBehaviour,ICollisionDetector
{
    private event Action<Collision> m_onPlateCollisionEnter;

    public event Action<Collision> OnCollisionEnterDetection
    {
        add
        {
            m_onPlateCollisionEnter += value;
        }
        remove
        {
            m_onPlateCollisionEnter -= value;
        }
    }

    private event Action<Collision> m_onPlateCollisionExit;

    public event Action<Collision> OnCollisionExitDetection
    {
        add
        {
            m_onPlateCollisionExit += value;
        }
        remove
        {
            m_onPlateCollisionExit -= value;
        }
    }
    private event Action<Collider> m_onPlateTriggerEnter;

    public event Action<Collider> OnTriggerEnterDetection
    {
        add
        {
            m_onPlateTriggerEnter += value;
        }
        remove
        {
            m_onPlateTriggerEnter -= value;
        }
    }
    private event Action<Collider> m_onPlateTriggerExit;

    public event Action<Collider> OnTriggerExitDetection
    {
        add
        {
            m_onPlateTriggerExit += value;
        }
        remove
        {
            m_onPlateTriggerExit -= value;
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        m_onPlateCollisionEnter?.Invoke(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        m_onPlateCollisionExit?.Invoke(collision);
    }

    private void OnTriggerEnter(Collider other)
    {
        m_onPlateTriggerEnter?.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        m_onPlateTriggerExit?.Invoke(other);
    }
}
