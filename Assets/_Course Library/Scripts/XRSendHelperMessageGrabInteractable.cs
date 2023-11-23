using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSendHelperMessageGrabInteractable : XRGrabInteractable
{
    private Transform m_originalParent;



    protected override void Grab()
    {
        m_originalParent = transform.parent;
        base.Grab();

    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

        SelectEnterEventArgsWithParentInfo argsWithParent = new SelectEnterEventArgsWithParentInfo(args, m_originalParent);

        HelperManager.m_Instance.SendMessage("ObjectSelected", argsWithParent, SendMessageOptions.DontRequireReceiver);
        base.OnSelectEntered(args);
    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
        SelectExitEventArgsWithParentInfo argsWithParent = new SelectExitEventArgsWithParentInfo(args, m_originalParent);
        argsWithParent.m_originalInteractableParent = m_originalParent;
        HelperManager.m_Instance.SendMessage("ObjectDeselected", argsWithParent, SendMessageOptions.DontRequireReceiver);
    }
}

public class SelectEnterEventArgsWithParentInfo
{
    public Transform m_originalInteractableParent=null;

    public SelectEnterEventArgs args = null;
    public SelectEnterEventArgsWithParentInfo(SelectEnterEventArgs selectEnargsterEventArgs, Transform parent)
    {
        m_originalInteractableParent = parent;
        args = selectEnargsterEventArgs;
    }
}
public class SelectExitEventArgsWithParentInfo
{
    public Transform m_originalInteractableParent = null;
    public SelectExitEventArgs args = null;

    public SelectExitEventArgsWithParentInfo(SelectExitEventArgs selectExitEventArgs, Transform parent)
    {
        m_originalInteractableParent = parent;
        args = selectExitEventArgs;
    }
}

