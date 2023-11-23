using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteFire : MonoBehaviour
{

    [SerializeField] ParticleSystem m_Fire;
    // Start is called before the first frame update


    private void OnParticleCollision(GameObject other)
    {
        if (other.tag=="FireFlame" && m_Fire.isStopped)
        {
            m_Fire.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "FireFlame" && m_Fire.isStopped)
        {
            m_Fire.Play();
        }
    }
}
