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
        Debug.Log("Enter second state");
        m_keyhole.SetCollisionBetweenKeyAndKeyholeActive(true);
        m_keyhole.SetDragParametersKeyInKeyhole(true);

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
        Debug.Log(" " + m_keyJoint.connectedAnchor);
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
        m_keyhole.KeyTransform.rotation = m_attachedRotation;
        m_keyhole.KeyRigidbody.angularVelocity = Vector3.zero;

        Vector3 localVelocity = m_keyhole.KeyTransform.InverseTransformDirection(m_keyhole.KeyRigidbody.velocity);
        localVelocity.x = 0;
        localVelocity.y = 0;

        m_keyhole.KeyRigidbody.velocity = m_keyhole.KeyTransform.TransformDirection(localVelocity);

        
        var relativeStartPosition = m_attachedTransform.InverseTransformPoint(m_keyhole.KeyTransform.position);
        var updatedrelativePosition = new Vector3(0, 0, relativeStartPosition.z);
        var globalPosition = m_attachedTransform.TransformPoint(updatedrelativePosition);
        m_keyhole.KeyTransform.transform.position = globalPosition;
        */
    }
    public void Exit()
    {
        Debug.Log("Exit second state");
        Object.Destroy(m_keyJoint);
    }

    
}
