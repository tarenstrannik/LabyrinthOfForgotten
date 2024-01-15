using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    [SerializeField] private AudioSource m_audioSource = null;

    [SerializeField] private AudioClip[] m_clips;

    private void Awake()
    {
        if (m_audioSource == null)
            m_audioSource = GetComponentInChildren<AudioSource>();
    }

    public void PlaySoundAtIndex(int index)
    {
        if (index >= 0 && index < m_clips.Length)
        {
            m_audioSource?.PlayOneShot(m_clips[index]);
        }
    }

    public void PlayRandomClip()
    {
        var randIndex=Random.Range(0, m_clips.Length);
        PlaySoundAtIndex(randIndex);
    }

    public void PlayAudioSourceClip()
    {
        m_audioSource?.Play();
    }
    public void PauseAudioSourceClip()
    {
        m_audioSource?.Pause();
    }
    public void StopAudioSourceClip()
    {
        m_audioSource?.Stop();
    }
    public void SetAudioSourcePitch(float value)
    {
        if(m_audioSource!=null)
            m_audioSource.pitch = value;
    }

   
}
