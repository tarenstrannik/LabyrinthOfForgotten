using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
public class KeyholeStateKeyIsInsertedAndCanRotate : IState
{
    private readonly KeyholeSocket m_keyhole;
    private readonly Transform m_attachTransform;
    private readonly Vector3 m_attachTransformPosition;
    private readonly Rigidbody m_keyholeRigidbody;
    private ConfigurableJoint m_keyJoint;
    public KeyholeStateKeyIsInsertedAndCanRotate(KeyholeSocket keyhole, Transform attachTransform, Vector3 attachTransformPosition, Rigidbody keyholeRigidbody)
    {
        m_keyhole = keyhole;
        m_attachTransform = attachTransform;
        m_attachTransformPosition = attachTransformPosition;
        m_keyholeRigidbody = keyholeRigidbody;
    }
    public void Enter()
    {
        Debug.Log("Enter fourth state");
        
    }
    public void Process()
    {
        m_keyhole.KeyTransform.position = m_attachTransformPosition;
        m_keyhole.KeyRigidbody.velocity = Vector3.zero;
    }
    public void Exit()
    {
        Object.Destroy(m_keyJoint);
    }
}
