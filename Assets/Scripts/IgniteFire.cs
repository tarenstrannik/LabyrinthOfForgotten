using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniteFire : MonoBehaviour
{

    [SerializeField] ParticleSystem m_Fire;
    public ParticleSystem Fire
    { get { return m_Fire; } }
    [SerializeField] ParticleSystem m_Smoke;
    // Start is called before the first frame update
    [SerializeField] AudioSource m_fireAudio;
    [SerializeField] AudioSource m_fireOneShotAudio;
    [SerializeField] AudioClip m_fireFadingAudio;
    private bool m_isFireStopped;
    private void Awake()
    {
        m_isFireStopped = m_Fire.isStopped;
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.tag=="FireFlame" && m_isFireStopped)
        {
            m_isFireStopped = false;
            m_Fire.Play();
            
            GetComponent<SaveLinkOnControllerSelectedObject>().Controller.SendMessage("AddPlayerFiringTorchAndEnableWaterCollider", gameObject,SendMessageOptions.DontRequireReceiver);
        }
        else if(other.tag =="Water" && !m_isFireStopped)
        {
            StopFire();
            m_isFireStopped = true;
            GetComponent<SaveLinkOnControllerSelectedObject>().Controller.SendMessage("RemovePlayerFiringTorchAndDisableWaterCollider", gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "FireFlame" && m_isFireStopped)
        {
            m_isFireStopped = false;
            m_Fire.Play();
            if(GetComponent<SaveLinkOnControllerSelectedObject>().Controller!=null) 
                GetComponent<SaveLinkOnControllerSelectedObject>().Controller.SendMessage("AddPlayerTorchAndEnableWaterCollider", gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void StopFire()
    {
        m_isFireStopped = true;
        m_Fire.Stop();
        m_Smoke.Play();
        m_fireAudio.Stop();
        m_fireOneShotAudio.volume = 1;
        m_fireOneShotAudio.PlayOneShot(m_fireFadingAudio);
    }
}
