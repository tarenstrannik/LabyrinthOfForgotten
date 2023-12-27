using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.XR.CoreUtils;
using UnityEngine;

public class IgniteFire : MonoBehaviour
{

    [SerializeField] ParticleSystem m_Fire;
    public ParticleSystem Fire
    { get { return m_Fire; } }

    [SerializeField] private Collider m_currentFlameCollider;

    [SerializeField] ParticleSystem m_Smoke;
    // Start is called before the first frame update
    [SerializeField] AudioSource m_fireAudio;
    [SerializeField] AudioSource m_fireOneShotAudio;
    [SerializeField] AudioClip m_fireFadingAudio;

    [SerializeField] private Light m_fireLight;
    public Light FireLight
    {
        get
        {
            return m_fireLight;
        }
    }
    private LightVariation m_lightVariation;

    [SerializeField] MeshRenderer m_FireRug;
    [SerializeField] Material m_firing;
    [SerializeField] Material m_noFiring;

    [SerializeField] private int m_collisionLayerInLeftHand;
    [SerializeField] private int m_collisionLayerInRightHand;

    [SerializeField] private int m_collisionLayerToExcludeBodyIfFiringTorchInLeftHand;
    [SerializeField] private int m_collisionLayerToExcludeBodyIfFiringTorchInRightHand;

    [SerializeField] private int m_collisionLayerInLeftRayHand;
    [SerializeField] private int m_collisionLayerInRightRayHand;

    [SerializeField] private int m_collisionLayerToExcludeBodyIfFiringTorchInLeftRayHand;
    [SerializeField] private int m_collisionLayerToExcludeBodyIfFiringTorchInRightRayHand;

    [SerializeField] private int m_collisionLayerToExcludeBodyIfInBackpack;
    [SerializeField] private int m_collisionLayerToExcludeBodyIfFiringTorchInBackpack;

    [SerializeField] private int m_defaultCollisionLayer;
    [SerializeField] private int m_defaultFireCollisionLayer;

    private bool m_isFireStopped;

    public bool IsFireStopped
    {
        get
        {
            return m_isFireStopped;
        }
    }
    [SerializeField] private string m_torchTag = "Torch";
    private GameObject m_torch;

    [SerializeField] private SaveLinkOnControllerSelectedObject m_controllerSaver;

    private void Awake()
    {
        m_isFireStopped = m_Fire.isStopped;
        if (m_fireLight != null) m_lightVariation = m_fireLight.GetComponent<LightVariation>();
        if (transform.parent && transform.parent.gameObject.CompareTag(m_torchTag))
        {
            m_torch = transform.parent.gameObject;
        }
        else
        {
            m_torch = gameObject;
        }

    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.tag == "FireFlame" && m_isFireStopped)
        {
            StartFire();

        }
        else if (other.tag == "Water" && !m_isFireStopped)
        {
            StopFire();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "FireFlame" && m_isFireStopped)
        {
            StartFire();
        }
    }

    private void StopFire()
    {
        m_isFireStopped = true;
        if (m_fireLight != null) m_fireLight.enabled = false;
        if (m_currentFlameCollider != null) m_currentFlameCollider.enabled = false;
        if (m_lightVariation != null) m_lightVariation.StopLightVariation();
        if (m_FireRug != null) m_FireRug.material = m_noFiring;
        m_Fire.Stop();
        if (m_Smoke != null) m_Smoke.Play();
        if (m_fireAudio != null) m_fireAudio.Stop();
        if (m_fireOneShotAudio != null)


        {
            m_fireOneShotAudio.volume = 1;
            m_fireOneShotAudio.PlayOneShot(m_fireFadingAudio);
        }
        if (m_controllerSaver != null && m_controllerSaver.Controller != null)
        {
            m_controllerSaver.Controller.SendMessage("RemovePlayerFiringTorchAndDisableWaterCollider", m_torch, SendMessageOptions.DontRequireReceiver);
        }
        ChangeLayer();
    }

    private void StartFire()
    {
        m_isFireStopped = false;
        m_Fire.Play();
        if (m_currentFlameCollider != null) m_currentFlameCollider.enabled = true;
        if (m_fireAudio != null) m_fireAudio.Play();
        if (m_FireRug != null) m_FireRug.material = m_firing;
        if (m_fireLight != null) m_fireLight.enabled = true;
        if (m_lightVariation != null) m_lightVariation.StartLightVariation();
        if (m_controllerSaver != null && m_controllerSaver.Controller != null)
        {
            m_controllerSaver.Controller.SendMessage("AddPlayerFiringTorchAndEnableWaterCollider", m_torch, SendMessageOptions.DontRequireReceiver);
        }
        ChangeLayer();

    }

    private void ChangeLayer()
    {
        GameObject objectToChangeLayerRec;
        if (transform.parent && transform.parent.gameObject.CompareTag(m_torchTag))
        {
            objectToChangeLayerRec = transform.parent.gameObject;
        }
        else
        {
            objectToChangeLayerRec = gameObject;
        };
        

        if (objectToChangeLayerRec.layer == m_collisionLayerInLeftHand)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInLeftHand);
        }
        else if (objectToChangeLayerRec.layer == m_collisionLayerToExcludeBodyIfFiringTorchInLeftHand)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerInLeftHand);
        }

        if (objectToChangeLayerRec.layer == m_collisionLayerInRightHand)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInRightHand);
        }
        else if (objectToChangeLayerRec.layer == m_collisionLayerToExcludeBodyIfFiringTorchInRightHand)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerInRightHand);
        }

        if (objectToChangeLayerRec.layer == m_collisionLayerInLeftRayHand)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInLeftRayHand);
        }
        else if (objectToChangeLayerRec.layer == m_collisionLayerToExcludeBodyIfFiringTorchInLeftRayHand)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerInLeftRayHand);
        }

        if (objectToChangeLayerRec.layer == m_collisionLayerInRightRayHand)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInRightRayHand);
        }
        else if (objectToChangeLayerRec.layer == m_collisionLayerToExcludeBodyIfFiringTorchInRightRayHand)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerInRightRayHand);
        }


        else if (objectToChangeLayerRec.layer == m_collisionLayerToExcludeBodyIfInBackpack)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInBackpack);
        }
        else if (objectToChangeLayerRec.layer == m_collisionLayerToExcludeBodyIfFiringTorchInBackpack)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_collisionLayerToExcludeBodyIfInBackpack);
        }
        else if (objectToChangeLayerRec.layer == m_defaultCollisionLayer)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_defaultFireCollisionLayer);
        }
        else if (objectToChangeLayerRec.layer == m_defaultFireCollisionLayer)
        {
            objectToChangeLayerRec.SetLayerRecursively(m_defaultCollisionLayer);
        }
    }


}
