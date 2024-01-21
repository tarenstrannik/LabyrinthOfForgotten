using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;
public class WaterfallForTorches : MonoBehaviour
{
    //[SerializeField] private UnityEvent m_activatedEvent;
    //[SerializeField] private UnityEvent m_deactivatedEvent;

    private ParticleSystem m_particles;
    private AudioSource m_audio;

    private void Awake()
    {
        m_particles=GetComponentInChildren<ParticleSystem>();
        m_audio=GetComponentInChildren<AudioSource>();
    }

    public void Activate()
    {
        //m_activatedEvent.Invoke();
        m_particles.Play();
        m_audio.Play();
    }
    public void Dectivate()
    {
        //m_deactivatedEvent.Invoke();
        m_particles.Stop();
        m_audio.Stop();
    }
}
