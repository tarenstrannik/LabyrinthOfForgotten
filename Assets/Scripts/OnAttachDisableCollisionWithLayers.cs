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

    public void Attach(SelectEnterEventArgs args)
    {
        if(args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>()!=null && !args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().IsFireStopped)
        {
            args.interactableObject.transform.gameObject.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorch);
        }
        else
        {
            args.interactableObject.transform.gameObject.SetLayerRecursively(m_collisionLayerToExcludeBody);
        }
        
    }

    public void Detach(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>() != null && !args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().IsFireStopped)
        {
            args.interactableObject.transform.gameObject.SetLayerRecursively(m_defaultFireCollisionLayer);
        }
        else
        {
            args.interactableObject.transform.gameObject.SetLayerRecursively(m_defaultCollisionLayer);
        }
        
    }
}
