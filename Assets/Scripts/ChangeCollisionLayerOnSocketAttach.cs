using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ChangeCollisionLayerOnSocketAttach : MonoBehaviour
{
    private int m_curCollisionLayer;
    [SerializeField] private int m_targetCollisionLayer;
    // Start is called before the first frame update
    public void ChangeCollisionLayer(SelectEnterEventArgs args)
    {
        m_curCollisionLayer = args.interactableObject.transform.gameObject.layer;
        args.interactableObject.transform.gameObject.layer = m_targetCollisionLayer;

    }

    public void UnchangeCollisionLayer(SelectExitEventArgs args)
    {
        args.interactableObject.transform.gameObject.layer = m_curCollisionLayer;
    }


}
