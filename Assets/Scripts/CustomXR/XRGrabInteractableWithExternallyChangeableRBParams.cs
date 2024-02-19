using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableWithExternallyChangeableRBParams : XRGrabInteractable
{

    private bool m_rigidbodyParamsManuallyChangedState;

    private bool m_manualKinematic;
    private bool m_manualGravity;
    private float m_manualDrag;
    private float m_manualAngularDrag;

    private bool m_defaultKinematic;
    private bool m_defaultGravity;
    private float m_defaultDrag;
    private float m_defaultAngularDrag;

    private Rigidbody m_rb;
    protected override void Awake()
    {
        base.Awake();
        m_rb=GetComponent<Rigidbody>();
    }

    public void SaveDefaultRBParams(bool defaultKinematic,bool defaultGravity,float  defaultDrag,float defaultAngularDrag)
    {

        m_defaultKinematic = defaultKinematic;
        m_defaultGravity = defaultGravity;
        m_defaultDrag = defaultDrag;
        m_defaultAngularDrag = defaultAngularDrag;
    }

    public void SaveManualRBParams(bool manualKinematic, bool manualGravity, float manualDrag, float manualAngularDrag)
    {

        m_manualKinematic = manualKinematic;
        m_manualGravity = manualGravity;
        m_manualDrag = manualDrag;
        m_manualAngularDrag = manualAngularDrag;
    }

    protected override void SetupRigidbodyGrab(Rigidbody rigidbody)
    {
        if(!m_rigidbodyParamsManuallyChangedState)
        {
            SaveDefaultRBParams(rigidbody.isKinematic, rigidbody.useGravity, rigidbody.drag, rigidbody.angularDrag);
        }
        
        base.SetupRigidbodyGrab(rigidbody);
        
    }

    protected override void SetupRigidbodyDrop(Rigidbody rigidbody)
    {
        if(!m_rigidbodyParamsManuallyChangedState)
        {
            rigidbody.isKinematic = m_defaultKinematic;
            rigidbody.useGravity = m_defaultGravity;
            rigidbody.drag = m_defaultDrag;
            rigidbody.angularDrag = m_defaultAngularDrag;


            if (!isSelected)
                rigidbody.useGravity |= forceGravityOnDetach;
        }
        else
        {
            rigidbody.isKinematic = m_manualKinematic;
            rigidbody.useGravity = m_manualGravity;
            rigidbody.drag = m_manualDrag;
            rigidbody.angularDrag = m_manualAngularDrag;
        }

    }


    public void SetManualChangedStateActive(bool value)
    {
        m_rigidbodyParamsManuallyChangedState = value;
        if (interactorsSelecting.Count == 0)
        {
            if(value)
            {
                m_rb.isKinematic = m_manualKinematic;
                m_rb.useGravity = m_manualGravity;
                m_rb.drag = m_manualDrag;
                m_rb.angularDrag = m_manualAngularDrag;
            }
            else
            {
                m_rb.isKinematic = m_defaultKinematic;
                m_rb.useGravity = m_defaultGravity;
                m_rb.drag = m_defaultDrag;
                m_rb.angularDrag = m_defaultAngularDrag;
            }
        }
    }
}
