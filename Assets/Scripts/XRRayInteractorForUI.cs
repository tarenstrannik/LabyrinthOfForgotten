using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class XRRayInteractorForUI : XRRayInteractor
{
    [SerializeField] private string m_UIInteractableTag;
    private Gradient m_ValidColorGradient;
    private XRInteractorLineVisual m_LineVisual;

    protected override void Awake()
    {
        base.Awake();
        m_LineVisual=GetComponent<XRInteractorLineVisual>();
        m_ValidColorGradient = m_LineVisual.validColorGradient;


    }
    protected override void OnUIHoverEntered(UIHoverEventArgs args)
    {
        base.OnUIHoverEntered(args);
        
        if(args.uiObject.tag == m_UIInteractableTag)
        {
            m_LineVisual.validColorGradient = m_ValidColorGradient;
        }
        else
        {
            m_LineVisual.validColorGradient = m_LineVisual.invalidColorGradient;
        }
    }
}
