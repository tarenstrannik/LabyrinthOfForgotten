using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringBackpackBackIfAway : MonoBehaviour
{
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_maxBackpackDistance = 1.5f;
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.Distance(transform.position, m_target.position)>= m_maxBackpackDistance)
        {
            transform.position = m_target.position;
        }
    }
}
