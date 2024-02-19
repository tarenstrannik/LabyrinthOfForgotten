using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyholeStateKeyIsInsertedAndCanMoveRotate : IState
{
    private readonly KeyholeSocket m_keyhole;
    private readonly Quaternion m_attachedTransformRotation;
    private readonly Rigidbody m_keyholeRigidbody;
    private readonly float m_zLimit;
    private readonly float m_maxAngularZLimit;

    private ConfigurableJoint m_keyJoint;

    private Quaternion m_keyRotationOnEnter;
    public KeyholeStateKeyIsInsertedAndCanMoveRotate(KeyholeSocket keyhole, Quaternion attachedTransformRotation, Rigidbody keyholeRigidbody, float zLimit, float maxAngularZLimit)
    {
        m_keyhole = keyhole;
        m_attachedTransformRotation = attachedTransformRotation;
        m_keyholeRigidbody = keyholeRigidbody;
        m_zLimit = zLimit;
        m_maxAngularZLimit = maxAngularZLimit;

    }
    public void Enter()
    {
        m_keyRotationOnEnter = m_keyhole.KeyTransform.rotation;
        m_keyhole.KeyTransform.rotation = m_attachedTransformRotation;

        m_keyJoint = m_keyhole.KeyTransform.gameObject.AddComponent<ConfigurableJoint>();
        m_keyJoint.connectedBody = m_keyholeRigidbody;
        m_keyJoint.autoConfigureConnectedAnchor = false;
        m_keyJoint.connectedAnchor = new Vector3(0, 0, -m_zLimit/2);
        var limit = new SoftJointLimit();
        limit.limit = m_zLimit/2;
        m_keyJoint.linearLimit = limit;

        limit.limit = 0;
        m_keyJoint.lowAngularXLimit = limit;
        limit.limit = m_maxAngularZLimit;
        m_keyJoint.highAngularXLimit = limit;

        m_keyJoint.angularXMotion = ConfigurableJointMotion.Limited;
        m_keyJoint.angularYMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.angularZMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.xMotion = ConfigurableJointMotion.Limited;
        m_keyJoint.yMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.zMotion = ConfigurableJointMotion.Locked;
        
        m_keyJoint.axis = Vector3.forward;

        m_keyhole.KeyTransform.rotation = m_keyRotationOnEnter;
    }
    public void Process()
    {
    }
    public void Exit()
    {
        Object.Destroy(m_keyJoint);
    }
}
