using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningPartCollision : MonoBehaviour
{
    [SerializeField] private FireController m_fireController;
    private void OnParticleCollision(GameObject other)
    {
        m_fireController.OnParticleCollided(other);
    }

    private void OnTriggerEnter(Collider other)
    {

        m_fireController.OnTriggerEntered(other);
    }
}
