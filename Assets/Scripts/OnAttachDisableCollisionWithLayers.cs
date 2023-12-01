using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnAttachDisableCollisionWithLayers : MonoBehaviour
{
    [SerializeField] private int m_collisionLayerToExcludeBody;

    private int m_curCollisionLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Attach(SelectEnterEventArgs args)
    {
        m_curCollisionLayer = args.interactableObject.transform.gameObject.layer;
        args.interactableObject.transform.gameObject.SetLayerRecursively(m_collisionLayerToExcludeBody);
    }

    public void Detach(SelectExitEventArgs args)
    {
        args.interactableObject.transform.gameObject.SetLayerRecursively(m_curCollisionLayer);
    }
}
