
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayWooshSounds : MonoBehaviour
{
    [SerializeField] private AudioSource m_wooshSounds;
    [SerializeField] private ParticleSystem m_fire;
    // Start is called before the first frame update
    [Tooltip("The list of audio clips to play from")]
    public List<AudioClip> audioClips = new List<AudioClip>();
    private int index = 0;
    public void PlayRandomWooshClip()
    {
        if (m_fire.isPlaying)
        {
            index = Random.Range(0, audioClips.Count);
            PlayClip();
        }
    }

    private void PlayClip()
    {
        
        m_wooshSounds.PlayOneShot(audioClips[Mathf.Abs(index)]);
    }
}
