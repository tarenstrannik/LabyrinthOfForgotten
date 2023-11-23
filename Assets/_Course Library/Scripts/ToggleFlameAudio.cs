using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class ToggleFlameAudio : MonoBehaviour
{
    private ParticleSystem currentParticleSystem = null;
    private AudioSource currentFlameAudio = null;
    // Start is called before the first frame update
    void Start()
    {
        currentParticleSystem = GetComponent<ParticleSystem>();
        currentFlameAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        currentFlameAudio.enabled = currentParticleSystem.isPlaying;
    }
}
