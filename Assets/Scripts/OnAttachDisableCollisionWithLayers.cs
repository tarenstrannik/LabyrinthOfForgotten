using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.GPUSort;

public class OnAttachDisableCollisionWithLayers : MonoBehaviour
{
    [SerializeField] private int m_collisionLayerToExcludeBody;
    [SerializeField] private int m_collisionLayerToExcludeBodyIfFiringTorch;

    [SerializeField] private int m_defaultCollisionLayer;
    [SerializeField] private int m_defaultFireCollisionLayer;


    [SerializeField] private float m_changingDelay = 0.5f;
    [SerializeField] private float m_changingBackDelay = 0.5f;

    public void ChangeCollisionLayer(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>() != null)
        {

            if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>() != null && !args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().IsFireStopped)
            {
                args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>().SetCollisionLevel(m_collisionLayerToExcludeBodyIfFiringTorch, 0);
            }
            else
            {
                args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>().SetCollisionLevel(m_collisionLayerToExcludeBody, 0);
            }
        }
    }

    public void UnchangeCollisionLayer(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>() != null)
        {
            if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>() != null && !args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().IsFireStopped)
            {
                args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>().SetCollisionLevel(m_defaultFireCollisionLayer, 0);
            }
            else
            {
                args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>().SetCollisionLevel(m_defaultCollisionLayer, 0);
            }
        }
    }

    public void ChangeCollisionLayerWithDelay(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>() != null)
        {
            if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>() != null && !args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().IsFireStopped)
            {
                args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>().SetCollisionLevel(m_collisionLayerToExcludeBodyIfFiringTorch, m_changingDelay);
            }
            else
            {
                args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>().SetCollisionLevel(m_collisionLayerToExcludeBody, m_changingDelay);
            }
        }
    }

    public void UnchangeCollisionLayerWithDelay(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>() != null)
        {
            if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>() != null && !args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().IsFireStopped)
            {
                args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>().SetCollisionLevel(m_defaultFireCollisionLayer, m_changingBackDelay);
            }
            else
            {
                args.interactableObject.transform.gameObject.GetComponent<ChangeCollisionLevel>().SetCollisionLevel(m_defaultCollisionLayer, m_changingBackDelay);
            }
        }
    }



}

