using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableDisableJoint : MonoBehaviour
{
    [SerializeField] private Joint joint;
    private Rigidbody connectedBody;
    private Quaternion initialRotation_JointToConnected;
    private Vector3 m_BaseJointAxis;

    private void Awake()
    {
        joint = joint ? joint : GetComponent<Joint>();
        if (joint) connectedBody = joint.connectedBody;
        else Debug.LogError("No joint found.", this);
        m_BaseJointAxis = joint.axis;
        
        Vector3 localAxis = transform.InverseTransformDirection(transform.parent.transform.TransformDirection(m_BaseJointAxis));
        joint.axis = localAxis;

        initialRotation_JointToConnected = Quaternion.Inverse(joint.transform.rotation) * joint.connectedBody.transform.rotation; //Your new rotation according to the connected bodies coordinate plane. I do this on Awake, and cache it for as long as the object remains in the scene.
       
    }

    private void OnEnable() 
    {
        joint.connectedBody = connectedBody;
       
        Quaternion currentRotation = joint.transform.rotation; //caches the new rotation that you want the joint to have.
        joint.transform.rotation = joint.connectedBody.transform.rotation*Quaternion.Inverse(initialRotation_JointToConnected); //Rotates the cached initial rotation to once again match the coordinate plane of the connected body
        
        Vector3 localAxis = transform.InverseTransformDirection(transform.parent.transform.TransformDirection(m_BaseJointAxis));
        joint.axis = localAxis;
        
        //joint.axis = joint.axis; //Triggers a limit recalculation
        joint.transform.rotation = currentRotation; //Puts the joint object back to where you want it!
       

    }

    private void OnDisable()
    {
        joint.connectedBody = null;
        connectedBody.WakeUp();
    }
}
