using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyholeStateKeyIsInsertedAndCanMove : IState
{
    private readonly KeyholeSocket m_keyhole;
    private readonly Transform m_attachedTransform;
    private readonly Rigidbody m_keyholeRigidbody;
    private readonly Quaternion m_attachedRotation;
    private readonly float m_zLimit;
    private ConfigurableJoint m_keyJoint;
    public KeyholeStateKeyIsInsertedAndCanMove(KeyholeSocket keyhole, Transform attachedTransform,
        Quaternion attachedRotation, Rigidbody keyholeRigidbody, float zLimit)
    {
        m_keyhole = keyhole;
        m_attachedTransform = attachedTransform;
        m_attachedRotation = attachedRotation;
        m_keyholeRigidbody = keyholeRigidbody;
        m_zLimit = zLimit;
    }
    public void Enter()
    {
        m_keyhole.SetKeyInKeyhole(true);
        
        //joint solution
        m_keyhole.KeyTransform.rotation = m_attachedRotation;
        
        var relativeOnEnterKeyPosition = m_attachedTransform.InverseTransformPoint(m_keyhole.KeyTransform.position);
        var updatedrelativeKeyPosition = new Vector3(0, 0, relativeOnEnterKeyPosition.z);
        var globalPosition = m_attachedTransform.TransformPoint(updatedrelativeKeyPosition);
        m_keyhole.KeyTransform.position = globalPosition;
        
        m_keyJoint = m_keyhole.KeyTransform.gameObject.AddComponent<ConfigurableJoint>();
        m_keyJoint.connectedBody = m_keyholeRigidbody;
        m_keyJoint.autoConfigureConnectedAnchor = false;

        
        m_keyJoint.connectedAnchor =  new Vector3(0, 0, -m_zLimit/2);

        var limit = new SoftJointLimit();
        limit.limit = m_zLimit/2;
        m_keyJoint.linearLimit = limit;

        m_keyJoint.angularXMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.angularYMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.angularZMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.xMotion = ConfigurableJointMotion.Limited;
        m_keyJoint.yMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.zMotion = ConfigurableJointMotion.Locked;
        
        m_keyJoint.axis = Vector3.forward;

    }
    public void Process()
    {

    }
    public void Exit()
    {
        Object.Destroy(m_keyJoint);
    }

    
}
