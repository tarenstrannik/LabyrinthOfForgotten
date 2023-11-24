using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
[RequireComponent(typeof(AudioSource))] 

public class AudioPlayer : MonoBehaviour
{

    protected AudioSource audioSource;
    protected AudioClip audioClipFromRecord;
    [SerializeField] protected XRSocketInteractor recordInteractor;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        audioSource=GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void PlayRecord()
    {

        audioSource.Play();
    }

    public virtual void StopRecord()
    {
        audioSource.Stop();
    }

    public void GetAudioFromRecord()
    {
        audioClipFromRecord = recordInteractor.GetOldestInteractableSelected().transform.gameObject.GetComponent<AudioContainer>().AudioRecord;

        if (audioClipFromRecord != null) audioSource.clip= audioClipFromRecord;
    }
    public void ClearAudio()
    {
        audioSource.clip = null;
    }
}
