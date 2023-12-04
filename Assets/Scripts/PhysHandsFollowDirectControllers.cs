using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysHandsFollowDirectControllers : MonoBehaviour
{
    [SerializeField] protected Transform m_controllerToFollow;
    public Transform ControllerToFollow
    {
        get
        {
                return m_controllerToFollow;
        }
        set
        {
            m_controllerToFollow = value;
        }
    }
    protected Rigidbody m_handRb;


    protected virtual void Awake()
    {
        m_handRb = GetComponent<Rigidbody>();
    }

    protected virtual void FixedUpdate()
    {
        m_handRb.velocity= (m_controllerToFollow.position-transform.position)/ Time.fixedDeltaTime;
        //Debug.Log(m_controllerToFollow.position+" "+ transform.position+" "+m_handRb.velocity);
        Quaternion rotationDifference = m_controllerToFollow.rotation * Quaternion.Inverse(transform.rotation);

        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegree = angleInDegree*rotationAxis;

        m_handRb.angularVelocity = rotationDifferenceInDegree* Mathf.Deg2Rad / Time.fixedDeltaTime;
    }
}
