using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketInteractableMultiselect : XRSocketInteractor
{
    private Dictionary<XRGrabInteractable, bool> m_multiselectedObjects = new Dictionary<XRGrabInteractable, bool>();

    [SerializeField] private UnityEvent<SelectEnterEventArgs> m_onAddToSelection;
    [SerializeField] private UnityEvent<SelectExitEventArgs> m_onRemoveFromSelection;


    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        var obj = args.interactableObject as XRGrabInteractable;
        m_multiselectedObjects.TryAdd(obj, false);
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        var obj = args.interactableObject as XRGrabInteractable;
        if (m_multiselectedObjects.ContainsKey(obj))
            m_multiselectedObjects.Remove(obj);
    }

    protected virtual void AddToSelection(XRGrabInteractable grabInteractable)
    {

        if (m_multiselectedObjects.ContainsKey(grabInteractable))
            m_multiselectedObjects[grabInteractable] = true;

        var args = new SelectEnterEventArgs();
        args.interactorObject = this;
        args.interactableObject = grabInteractable;

        m_onAddToSelection.Invoke(args);
    }


    protected virtual void RemoveFromSelection(XRGrabInteractable grabInteractable)
    {
        if (m_multiselectedObjects.ContainsKey(grabInteractable))
            m_multiselectedObjects[grabInteractable] = false;

        var args = new SelectExitEventArgs();
        args.interactorObject = this;
        args.interactableObject = grabInteractable;

        m_onRemoveFromSelection.Invoke(args);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return false;
    }

    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);
        foreach (var obj in m_multiselectedObjects)
        {
            if (ObjectWasAddedToSelection(obj.Key, obj.Value))//(obj.Key.interactorsSelecting.Count > 0 && obj.Value == false)
            {
                AddToSelection(obj.Key);
            }
                

            else if(ObjectWasRemovedSelection(obj.Key, obj.Value))
            {
                RemoveFromSelection(obj.Key);
            }
                

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed && m_multiselectedObjects[obj.Key]==true)
            {

            }
        }
        

    }
    private bool ObjectWasAddedToSelection(XRGrabInteractable grabInteractable, bool wasPreviouslySelected)
    {

        if (grabInteractable.interactorsSelecting.Count > 0 && wasPreviouslySelected == false)
        {
            return true;
        }
        return false;
    }
    private bool ObjectWasRemovedSelection(XRGrabInteractable grabInteractable, bool wasPreviouslySelected)
    {

        if (grabInteractable.interactorsSelecting.Count == 0 && wasPreviouslySelected == true)
        {
            return true;
        }
        return false;
    }
}
