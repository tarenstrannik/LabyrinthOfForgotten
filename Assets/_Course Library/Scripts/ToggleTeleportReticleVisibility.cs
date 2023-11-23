using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleTeleportReticleVisibility : MonoBehaviour
{
    private GameObject m_CustomReticle = null;

    private BaseTeleportationInteractable m_TeleportationArea;

    // Start is called before the first frame update
    void Awake()
    {
       
        m_TeleportationArea=GetComponent<BaseTeleportationInteractable>();
        m_CustomReticle = m_TeleportationArea.customReticle;

    }

    public void TurnOnTeleportReticle()
    {
        
        m_TeleportationArea.customReticle = m_CustomReticle;
    }
    public void TurnOffTeleportReticle()
    {
        
        m_TeleportationArea.customReticle = null;
    }

    /*private void OnEnable()
    {
        var args = new SelectEnterEventArgs();
        var args2 = new SelectExitEventArgs();
        GetComponent<TeleportationArea>().selectEntered.Invoke(args);
        GetComponent<TeleportationArea>().selectExited.Invoke(args2);
    }*/
}
