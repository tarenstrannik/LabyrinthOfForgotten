using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleTeleportRay : MonoBehaviour
{
    [SerializeField] private XRInteractorLineVisual m_Line=null;

    private void Awake()
    {
        m_Line=GetComponent<XRInteractorLineVisual>();
    }

    public void ToggleTeleportRayVisibility()
    {
        m_Line.enabled = !m_Line.enabled;
    }

    public void TurnOnTeleportRayVisibility()
    {
        m_Line.enabled = true;
        
    }
    public void TurnOffTeleportRayVisibility()
    {
        m_Line.enabled = false;
        
    }

    private void OnEnable()
    {
        TurnOffTeleportRayVisibility();
        
    }
}
