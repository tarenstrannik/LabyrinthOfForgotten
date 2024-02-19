using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class KeyholeStateKeyIsInsertedAndCanRotate : IState
{
    private readonly KeyholeSocket m_keyhole;
    private readonly Vector3 m_attachedTransformPosition;
    private readonly Vector3 m_attachedTransformRight;
    private readonly Quaternion m_attachedTransformRotation;
    private readonly Rigidbody m_keyholeRigidbody;

    private readonly float m_maxAngularZLimit;
    private readonly float m_angularZActivationLimit;

    private ConfigurableJoint m_keyJoint;
    private Quaternion m_keyRotationOnEnter;

    private bool m_isMaxReached;

    public event Action OnMaxReached;
    public event Action OnMaxLeft;
    public KeyholeStateKeyIsInsertedAndCanRotate(KeyholeSocket keyhole, Vector3 attachTransformPosition, Vector3 attachTransformRight,
        Quaternion attachedTransformRotation, Rigidbody keyholeRigidbody, float maxAngularZLimit, float angularZActivationLimit)
    {
        m_keyhole = keyhole;
        m_attachedTransformPosition = attachTransformPosition;
        m_attachedTransformRight=attachTransformRight;
        m_keyholeRigidbody = keyholeRigidbody;
        m_attachedTransformRotation = attachedTransformRotation;
        m_maxAngularZLimit = maxAngularZLimit;
        m_angularZActivationLimit = angularZActivationLimit;
    }
    public void Enter()
    {
        m_keyhole.KeyTransform.position = m_attachedTransformPosition;
        m_keyRotationOnEnter = m_keyhole.KeyTransform.rotation;
        m_keyhole.KeyTransform.rotation = m_attachedTransformRotation;

        m_keyJoint = m_keyhole.KeyTransform.gameObject.AddComponent<ConfigurableJoint>();
        m_keyJoint.connectedBody = m_keyholeRigidbody;
        m_keyJoint.autoConfigureConnectedAnchor = false;
        m_keyJoint.connectedAnchor = new Vector3(0, 0, 0);
        var limit = new SoftJointLimit();

        limit.limit = 0;
        m_keyJoint.lowAngularXLimit = limit;
        limit.limit = m_maxAngularZLimit;
        m_keyJoint.highAngularXLimit = limit;

        m_keyJoint.angularXMotion = ConfigurableJointMotion.Limited;
        m_keyJoint.angularYMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.angularZMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.xMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.yMotion = ConfigurableJointMotion.Locked;
        m_keyJoint.zMotion = ConfigurableJointMotion.Locked;

        m_keyJoint.axis = Vector3.forward;

        m_keyhole.KeyTransform.rotation = m_keyRotationOnEnter;
    }
    public void Process()
    {
        if (!m_isMaxReached && Vector3.Angle(m_keyhole.KeyTransform.right, m_attachedTransformRight) >= m_angularZActivationLimit)
        {
            OnMaxReached?.Invoke();
            m_isMaxReached = true;
            return; 
        }
        if(m_isMaxReached && Vector3.Angle(m_keyhole.KeyTransform.right, m_attachedTransformRight) < m_angularZActivationLimit)
        {
            OnMaxLeft?.Invoke();
            m_isMaxReached = false;
        }
    }
    public void Exit()
    {
        UnityEngine.Object.Destroy(m_keyJoint);
    }
}
