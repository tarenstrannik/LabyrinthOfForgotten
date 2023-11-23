using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ONSPAudioSource))]
public class CollisionSound : MonoBehaviour
{
    private AudioSource m_AudioSource;
    [SerializeField] private AudioClip m_collisionSound;
    [SerializeField] private float m_MaxSoundVelocity=5f;

    void Start()
    {
        m_AudioSource=GetComponent<AudioSource>();
        m_AudioSource.playOnAwake = false;
        m_AudioSource.loop = false;
        m_AudioSource.spatialize = true;
        m_AudioSource.spatialBlend = 1;
        m_AudioSource.spread = 120;

        var onspAudioSource = GetComponent<ONSPAudioSource>();
        onspAudioSource.EnableSpatialization = true;
        onspAudioSource.EnableRfl = true;
        onspAudioSource.UseInvSqr = true;

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        float collisionSpeed = collision.relativeVelocity.magnitude;

        float volumeScale = (collisionSpeed / m_MaxSoundVelocity)<=1 ? (collisionSpeed / m_MaxSoundVelocity) : 1;
        
        if (m_collisionSound != null) m_AudioSource.PlayOneShot(m_collisionSound, volumeScale);
    }
}
