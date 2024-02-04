using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollisionSound : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    
    [SerializeField] private float m_maxSoundVelocity=5f;

    private Materials m_material= Materials.none;

    //костыль. xr kit от юнити позволяет проталкивать объект в коллайдер, что вызывает множественное срабатывание звука коллизии
    [SerializeField] private float m_repeatCollisionDelay=0.5f;
    private float m_curCollisionDelay = 0f;

    void Start()
    {
        if(m_AudioSource==null) m_AudioSource =GetComponent<AudioSource>();
        m_AudioSource.playOnAwake = false;
        m_AudioSource.loop = false;
        m_AudioSource.spatialize = true;
        m_AudioSource.spatialBlend = 1;
        m_AudioSource.spread = 120;

        
        var objMat = GetComponent<ObjectMaterialSounds>();
        if(objMat!=null) m_material = GetComponent<ObjectMaterialSounds>().ObjectMaterial;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_curCollisionDelay <= 0 )
        {
            StartCoroutine(DecreaseDelay());
            float collisionSpeed = collision.relativeVelocity.magnitude;

            float volumeScale = (collisionSpeed / m_maxSoundVelocity) <= 1 ? (collisionSpeed / m_maxSoundVelocity) : 1;
            var target = collision.gameObject.GetComponent<ObjectMaterialSounds>();
            Materials targetMaterial = Materials.none;
            if (target != null) targetMaterial = target.ObjectMaterial;
            var curCollisionSound = AudioParametersManager.Instance.GetCollisionSound(m_material, targetMaterial);
            if (curCollisionSound != null && m_AudioSource != null) m_AudioSource.PlayOneShot(curCollisionSound, volumeScale);
        }
        
    }
  
    private IEnumerator DecreaseDelay()
    {
        m_curCollisionDelay = m_repeatCollisionDelay;
        while(m_curCollisionDelay>0)
        {
            m_curCollisionDelay -= Time.deltaTime;
            yield return null;
        }
    }
}
