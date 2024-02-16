using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyholeStateKeyIsInsertedAndCanMoveRotate : IState
{
    private readonly KeyholeSocket m_keyhole;
    private readonly Transform m_attachedTransform;
    private readonly Rigidbody m_keyholeRigidbody;
    private readonly float m_zLimit;

    private ConfigurableJoint m_keyJoint;
    public KeyholeStateKeyIsInsertedAndCanMoveRotate(KeyholeSocket keyhole, Transform attachTransform, Rigidbody keyholeRigidbody, float zLimit)
    {
        m_keyhole = keyhole;
        m_attachedTransform = attachTransform;
        m_keyholeRigidbody = keyholeRigidbody;
        m_zLimit = zLimit;
    }
    public void Enter()
    {
        Debug.Log("Enter third state");
        m_keyhole.KeyTransform.position = m_attachedTransform.position;
        m_keyhole.KeyTransform.rotation = m_attachedTransform.rotation;


        m_keyJoint = m_keyhole.KeyTransform.gameObject.AddComponent<ConfigurableJoint>();
        m_keyJoint.connectedBody = m_keyholeRigidbody;
        m_keyJoint.autoConfigureConnectedAnchor = false;
        m_keyJoint.connectedAnchor = new Vector3(0, 0, -m_zLimit/2);
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
        /*
        Vector3 localVelocity = m_keyhole.KeyTransform.InverseTransformDirection(m_keyhole.KeyRigidbody.velocity);
        localVelocity.x = 0;
        localVelocity.y = 0;

        m_keyhole.KeyRigidbody.velocity = m_keyhole.KeyTransform.TransformDirection(localVelocity);


        var relativeStartPosition = m_attachedTransform.InverseTransformPoint(m_keyhole.KeyTransform.position);
        var newRelativeZ = relativeStartPosition.z <= 0 ? relativeStartPosition.z : 0;
        var updatedrelativePosition = new Vector3(0, 0, newRelativeZ);
        var globalPosition = m_attachedTransform.TransformPoint(updatedrelativePosition);
        m_keyhole.KeyTransform.transform.position = globalPosition;
        */
    }
    public void Exit()
    {
        Object.Destroy(m_keyJoint);
    }
}
