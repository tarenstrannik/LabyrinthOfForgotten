using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOnTriggerEnter : MonoBehaviour
{
    [SerializeField] protected ToggleWaterfallColliderTemporaly m_parentScript;
    [SerializeField] protected int m_playerTeleportHandLayer=11;
    [SerializeField] protected int m_playerTeleportHandToFollowLayer = 9;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer== m_playerTeleportHandLayer)
        {
            m_parentScript.IsHandHere = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == m_playerTeleportHandLayer)
        {
            m_parentScript.IsHandHere = false;
        }
    }
}
