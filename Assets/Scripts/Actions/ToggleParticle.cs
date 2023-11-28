using UnityEngine;

/// <summary>
/// Toggles particle system
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
public class ToggleParticle : MonoBehaviour
{
    private ParticleSystem currentParticleSystem = null;
    private MonoBehaviour currentOwner = null;

    private AudioSource m_audioSource;
    private void Awake()
    {
        currentParticleSystem = GetComponent<ParticleSystem>();
        m_audioSource = GetComponent<AudioSource>();
    }

    public void Play()
    {
        currentParticleSystem.Play();
    }

    public void Stop()
    {
        currentParticleSystem.Stop();
    }

    public void PlayWithExclusivity(MonoBehaviour owner)
    {
        if(currentOwner == null)
        {
            currentOwner = this;
            Play();
            if (m_audioSource != null) m_audioSource.Play();
        }
    }

    public void StopWithExclusivity(MonoBehaviour owner)
    {
        if(currentOwner == this)
        {
            currentOwner = null;
            Stop();
            if (m_audioSource != null) m_audioSource.Pause();
        }
    }

    private void OnValidate()
    {
        if(currentParticleSystem)
        {
            ParticleSystem.MainModule main = currentParticleSystem.main;
            main.playOnAwake = false;
        }
    }
}
