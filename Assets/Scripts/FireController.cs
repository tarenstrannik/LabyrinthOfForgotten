using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

public class FireController : MonoBehaviour, ICanBlockTeleportIfActiveAndInHand
{

    [SerializeField] ParticleSystem m_fireParticles;

    [SerializeField] private Collider m_currentFlameCollider;

    [SerializeField] ParticleSystem m_smokeParticles;
    [SerializeField] AudioSource m_fireAudio;
    [SerializeField] AudioSource m_fireOneShotAudio;
    [SerializeField] AudioClip m_fireFadingAudioClip;

    [SerializeField] private Light m_fireLight;
    public Light FireLight
    {
        get
        {
            return m_fireLight;
        }
    }
    private LightVariation m_lightVariation;

    [SerializeField] MeshRenderer m_fireRugRenderer;
    [SerializeField] Material m_firingRugMaterial;
    [SerializeField] Material m_noFiringRugMaterial;

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

   
    [SerializeField] private bool m_isFireActive;

    public bool IsActive
    {
        get
        {
            return m_isFireActive;
        }
        private set
        {
            m_isFireActive = value;
        }
    }

    [SerializeField] private string m_fireParticlesTag = "FireFlame";
    [SerializeField] private string m_waterTag = "Water";

    //events
    [SerializeField] private UnityEvent<GameObject> m_onFireUp;
    public UnityEvent<GameObject> OnActivated 
    { 
        get
        {
            return m_onFireUp;
        }
        
    }
    [SerializeField] private UnityEvent<GameObject> m_onFadeOff;
    public UnityEvent<GameObject> OnDeactivated 
    {
        get
        {
            return m_onFadeOff;
        }

    }

    private void Awake()
    {
        if (m_fireLight != null) m_lightVariation = m_fireLight.GetComponent<LightVariation>();

    }

    private void OnEnable()
    {
        if (IsActive)
        {
            m_fireParticles?.Play();
            m_fireAudio?.Play();
        }
        OnActivated.AddListener(StartFire);
        OnDeactivated.AddListener(StopFire);
    }
    private void OnDisable()
    {
        OnActivated.RemoveListener(StartFire);
        OnDeactivated.RemoveListener(StopFire);
    }
    public void OnParticleCollided(GameObject other)
    {
        if (!IsActive && other.CompareTag(m_fireParticlesTag))
        {
            OnActivated.Invoke(gameObject);

        }
        else if (IsActive && other.CompareTag(m_waterTag))
        {
            OnDeactivated.Invoke(gameObject);
        }
    }

    public void OnTriggerEntered(Collider other)
    {

        if (!IsActive && other.gameObject.CompareTag(m_fireParticlesTag))
        {
            OnActivated.Invoke(gameObject);
        }
    }

    private void StopFire(GameObject obj)
    {
        IsActive = false;
        if (m_fireLight != null) m_fireLight.enabled = false;
        if (m_currentFlameCollider != null) m_currentFlameCollider.enabled = false;
        if (m_lightVariation != null) m_lightVariation.StopLightVariation();
        if (m_fireRugRenderer != null) m_fireRugRenderer.material = m_noFiringRugMaterial;
        m_fireParticles.Stop();
        if (m_smokeParticles != null) m_smokeParticles.Play();
        if (m_fireAudio != null) m_fireAudio.Stop();
        if (m_fireOneShotAudio != null)
        {
            m_fireOneShotAudio.volume = 1;
            m_fireOneShotAudio.PlayOneShot(m_fireFadingAudioClip);
        }
        ChangeLayer();
        
    }

    private void StartFire(GameObject obj)
    {
        IsActive = true;
        m_fireParticles.Play();
        if (m_currentFlameCollider != null) m_currentFlameCollider.enabled = true;
        if (m_fireAudio != null) m_fireAudio.Play();
        if (m_fireRugRenderer != null) m_fireRugRenderer.material = m_firingRugMaterial;
        if (m_fireLight != null) m_fireLight.enabled = true;
        if (m_lightVariation != null) m_lightVariation.StartLightVariation();
        ChangeLayer();
        
    }

    private void ChangeLayer()
    {
        if (gameObject.layer == m_collisionLayerInLeftHand)
        {
            gameObject.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInLeftHand);
        }
        else if (gameObject.layer == m_collisionLayerToExcludeBodyIfFiringTorchInLeftHand)
        {
            gameObject.SetLayerRecursively(m_collisionLayerInLeftHand);
        }

        else if (gameObject.layer == m_collisionLayerInRightHand)
        {
            gameObject.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInRightHand);
        }
        else if (gameObject.layer == m_collisionLayerToExcludeBodyIfFiringTorchInRightHand)
        {
            gameObject.SetLayerRecursively(m_collisionLayerInRightHand);
        }

        else if(gameObject.layer == m_collisionLayerInLeftRayHand)
        {
            gameObject.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInLeftRayHand);
        }
        else if (gameObject.layer == m_collisionLayerToExcludeBodyIfFiringTorchInLeftRayHand)
        {
            gameObject.SetLayerRecursively(m_collisionLayerInLeftRayHand);
        }

        else if(gameObject.layer == m_collisionLayerInRightRayHand)
        {
            gameObject.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInRightRayHand);
        }
        else if (gameObject.layer == m_collisionLayerToExcludeBodyIfFiringTorchInRightRayHand)
        {
            gameObject.SetLayerRecursively(m_collisionLayerInRightRayHand);
        }

        else if (gameObject.layer == m_collisionLayerToExcludeBodyIfInBackpack)
        {
            gameObject.SetLayerRecursively(m_collisionLayerToExcludeBodyIfFiringTorchInBackpack);
        }
        else if (gameObject.layer == m_collisionLayerToExcludeBodyIfFiringTorchInBackpack)
        {
            gameObject.SetLayerRecursively(m_collisionLayerToExcludeBodyIfInBackpack);
        }
        else if (gameObject.layer == m_defaultCollisionLayer)
        {
            gameObject.SetLayerRecursively(m_defaultFireCollisionLayer);
        }
        else if (gameObject.layer == m_defaultFireCollisionLayer)
        {
            gameObject.SetLayerRecursively(m_defaultCollisionLayer);
        }
    }


}
