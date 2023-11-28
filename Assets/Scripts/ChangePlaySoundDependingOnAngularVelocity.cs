using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlaySoundDependingOnAngularVelocity : MonoBehaviour
{
    [SerializeField] private AudioSource m_doorAudioSource;
    // Start is called before the first frame update

    private float m_startPitch;
    private float m_startVolume;

    private Rigidbody m_doorRb;
    [SerializeField] private float m_maxAngularVelocity;

    private Coroutine UpdatingParamsCoroutine = null;

    public void Awake()
    {
        //m_startPitch = m_doorAudioSource.pitch;
        m_startVolume = m_doorAudioSource.volume;
        m_doorRb = GetComponent<Rigidbody>();
    }

    public void StartUpdateAudioParams()
    {
        if(UpdatingParamsCoroutine==null)
            UpdatingParamsCoroutine= StartCoroutine(IUpdateAudioParams());
    }
    public void StopUpdateAudioParams()
    {
        if (UpdatingParamsCoroutine != null)
        {
            StopCoroutine(UpdatingParamsCoroutine);
            UpdatingParamsCoroutine = null;
        }
    }

    private IEnumerator IUpdateAudioParams()
    {
        while(true)
        {
            float volumeScale = (m_doorRb.angularVelocity.magnitude / m_maxAngularVelocity) <= 1 ? (m_doorRb.angularVelocity.magnitude / m_maxAngularVelocity) : 1;
            m_doorAudioSource.volume = m_startVolume * volumeScale;
            //m_doorAudioSource.pitch = (m_doorRb.angularVelocity.magnitude / m_maxAngularVelocity) * m_startPitch;

            yield return null;
        }
    }
}
