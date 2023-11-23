using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFlameCollider : MonoBehaviour
{
    private ParticleSystem currentParticleSystem = null;
    private Collider currentFlameCollider= null;
    // Start is called before the first frame update
    void Start()
    {
        currentParticleSystem = GetComponent<ParticleSystem>();
        currentFlameCollider=GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        currentFlameCollider.enabled = currentParticleSystem.isPlaying;
    }
}
