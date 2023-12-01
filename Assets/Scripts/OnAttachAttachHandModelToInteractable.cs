using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class OnAttachAttachHandModelToInteractable : MonoBehaviour
{
    [SerializeField] private string[] m_TagsToFollow;
    private Transform m_initialToFollow;
    // Start is called before the first frame update
    private PhysHandsFollowDirectControllers m_handsFollowDirectControllers;

    private bool m_isAttachChanged = false;

    private void Awake()
    {
        m_handsFollowDirectControllers = GetComponent<PhysHandsFollowDirectControllers>();
        m_initialToFollow = m_handsFollowDirectControllers.ControllerToFollow;
    }

    public void Attach(SelectEnterEventArgs args)
    {
        
        if(m_TagsToFollow.Contains(args.interactableObject.transform.tag))
        {
            m_isAttachChanged = true;
            m_handsFollowDirectControllers.ControllerToFollow = args.interactableObject.GetAttachTransform(args.interactorObject);
        }
    }

    public void Detach()
    {
        if(m_isAttachChanged)
        {
            m_handsFollowDirectControllers.ControllerToFollow = m_initialToFollow;
        }
    }
}


