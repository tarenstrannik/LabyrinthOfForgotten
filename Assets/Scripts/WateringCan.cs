using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EulerAngleFunctions;
public class WateringCan : MonoBehaviour
{
    [SerializeField][Range(0, 360)] public float m_MinMaxFloat = 0.0f;
    [SerializeField][Range(0, 360)] public float m_MaxMaxFloat = 0.0f;


    [Range(0, 360)] public float m_MinThreshold = 0.0f;
    [Range(0, 360)] public float m_MaxThreshold = 0.0f;

    private ParticleSystem m_WaterParticleSystem;
    private float m_MaxIntensity;


    [SerializeField] AudioSource m_PouringAudioSource;
    private float m_MaxPoringSoundVolume;
    private float m_MaxPoringSoundSpeed;
    private void Awake()
    {
        m_WaterParticleSystem = GetComponentInChildren<ParticleSystem>();

        m_MaxIntensity = m_WaterParticleSystem.emission.rateOverTimeMultiplier;

        m_MaxPoringSoundVolume = m_PouringAudioSource.volume;
        m_MaxPoringSoundSpeed = m_PouringAudioSource.pitch;
    }



    public void AdjustVolume()
    {
        m_PouringAudioSource.volume= m_MaxPoringSoundVolume* StrengthCoef();
    }
    public void AdjustSoundSpeed()
    {
        m_PouringAudioSource.pitch = m_MaxPoringSoundSpeed * StrengthCoef();
    }
    public void AdjustIntensity()
    {
        var emission = m_WaterParticleSystem.emission;
        emission.rateOverTimeMultiplier = m_MaxIntensity * StrengthCoef();
        
    }

    private float StrengthCoef()
    {
        float curEulerX = GetXDegrees(transform);
        if(curEulerX >= m_MinMaxFloat && curEulerX <= m_MaxMaxFloat)
        {
            return 1;
        }
       else if(curEulerX>= m_MinThreshold && curEulerX < m_MinMaxFloat)
        {
            return Mathf.InverseLerp(m_MinThreshold, m_MinMaxFloat, curEulerX); 
        }
        else if (curEulerX > m_MaxMaxFloat && curEulerX <= m_MaxThreshold)
        {
            return Mathf.InverseLerp(m_MaxThreshold, m_MaxMaxFloat, curEulerX);
        }
        else
        {
            return 0;
        }
    }

}
