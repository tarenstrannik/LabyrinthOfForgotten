using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RoomscaleFix : MonoBehaviour
{
    private CharacterController m_characterController;
    private XROrigin m_XROrigin;
    private CapsuleCollider m_physicalCollider;
    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
        m_XROrigin=GetComponent<XROrigin>();
        m_physicalCollider = GetComponent<CapsuleCollider>();
        m_physicalCollider.radius = m_characterController.radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        m_characterController.height = m_XROrigin.CameraInOriginSpaceHeight + 0.15f;
        m_physicalCollider.height = m_characterController.height + m_characterController.skinWidth;

        var centerPoint = transform.InverseTransformPoint(m_XROrigin.Camera.transform.position);

 
            m_characterController.center = new Vector3(centerPoint.x, m_characterController.height / 2 + m_characterController.skinWidth, centerPoint.z);
            m_characterController.Move(new Vector3(0.001f, -0.001f, 0.001f));
            m_characterController.Move(new Vector3(-0.001f, -0.001f, -0.001f));
            m_physicalCollider.center = new Vector3(m_characterController.center.x, m_physicalCollider.height / 2, m_characterController.center.z);

        

        
    }
}
