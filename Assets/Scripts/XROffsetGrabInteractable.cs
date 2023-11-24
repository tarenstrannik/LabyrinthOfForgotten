using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{
    private Vector3 m_InitialAttachLocalPos;
    private Quaternion m_InitialAttachLocalRot;
    // Start is called before the first frame update
    void Start()
    {
     if(!attachTransform)
        {
            GameObject grab = new GameObject("Grab pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }
        m_InitialAttachLocalPos = attachTransform.localPosition;
        m_InitialAttachLocalRot = attachTransform.localRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        
        if (args.interactorObject is XRDirectInteractor)
        {
            attachTransform.position = args.interactorObject.transform.position;
            attachTransform.rotation = args.interactorObject.transform.rotation;
        }
        else
        {
            attachTransform.localPosition= m_InitialAttachLocalPos;
            attachTransform.localRotation= m_InitialAttachLocalRot;
        }

        base.OnSelectEntered(args);


    }

}
