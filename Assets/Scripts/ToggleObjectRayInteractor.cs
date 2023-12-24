using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.GPUSort;

public class ToggleObjectRayInteractor : MonoBehaviour
{
    [SerializeField] private XRRayInteractor m_rayInteractor;

    [SerializeField] private bool m_isObjectInHand = false;

    public bool IsObjectInHand
    {
        get
        {
            return m_isObjectInHand;
        }
        set
        {
            m_isObjectInHand = value;
        }
    }


    [SerializeField] private bool m_isHoveringUI = false;

    public bool IsHoveringUI
    {
        get
        {
            return m_isHoveringUI;
        }
        set
        {
            m_isHoveringUI = value;
        }
    }


    [SerializeField]  private XRDirectInteractor m_directInteractor;

    private void Awake()
    {
        m_directInteractor = GetComponent<XRDirectInteractor>();
    }
    


    public void EnableRayInteractorIfNoObjectInHand(SelectExitEventArgs args)
    {
        if(!IsHoveringUI)
            m_rayInteractor.enabled=true;

    }

    public void DisableRayInteractorIfObjectInHand(SelectEnterEventArgs args)
    {

            if (args.interactableObject != null)
            {
                m_rayInteractor.enabled=false;
            }

    }


    public void DisableRayInteractorNoCondition()
    {

        m_rayInteractor.enabled=false;
        
    }
    public void ToggleRayInteractor(bool val)
    {
        if (!IsHoveringUI && m_directInteractor.interactablesSelected.Count == 0)
        {
            m_rayInteractor.enabled= val;
        }
        else
        {
            m_rayInteractor.enabled = false;
        }
    }


}
