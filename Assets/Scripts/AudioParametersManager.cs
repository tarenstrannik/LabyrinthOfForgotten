using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioParametersManager : MonoBehaviour
{
    private static AudioParametersManager m_instance;

    public static AudioParametersManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                GameObject hm = new GameObject("AudioParametersManager");
                hm.AddComponent<AudioParametersManager>();
            }
            return m_instance;
        }
    }
    [SerializeField] private AudioClip m_defaultSound;

    [SerializeField] private AudioClip m_defaultWoodSound;
    [SerializeField] private AudioClip m_defaultStoneSound;
    [SerializeField] private AudioClip m_defaultMetalSound;


    [SerializeField] private AudioClip m_woodToWoodSound;
    [SerializeField] private AudioClip m_woodToStoneSound;
    [SerializeField] private AudioClip m_woodToMetalSound;

    [SerializeField] private AudioClip m_stoneToWoodSound;
    [SerializeField] private AudioClip m_stoneToStoneSound;
    [SerializeField] private AudioClip m_stoneToMetalSound;

    [SerializeField] private AudioClip m_metalToWoodSound;
    [SerializeField] private AudioClip m_metalToStoneSound;
    [SerializeField] private AudioClip m_metalToMetalSound;

    private void Awake()
    {
        m_instance = this;
    }

    public AudioClip GetCollisionSound(Materials sourceMaterial, Materials targetMaterial)
    {
        if(sourceMaterial == Materials.wood)
        {
            if(targetMaterial==Materials.wood)
            {
                return m_woodToWoodSound;
            }
            else if (targetMaterial == Materials.stone)
            {
                return m_woodToStoneSound;
            }
            else if (targetMaterial == Materials.metall)
            {
                return m_woodToMetalSound;
            }
            else
            {
                return m_defaultWoodSound;
            }
        }

        else if (sourceMaterial == Materials.stone)
        {
            if (targetMaterial == Materials.wood)
            {
                return m_stoneToWoodSound;
            }
            else if (targetMaterial == Materials.stone)
            {
                return m_stoneToStoneSound;
            }
            else if (targetMaterial == Materials.metall)
            {
                return m_stoneToMetalSound;
            }
            else
            {
                return m_defaultStoneSound;
            }
        }

        else if (sourceMaterial == Materials.metall)
        {
            if (targetMaterial == Materials.wood)
            {
                return m_metalToWoodSound;
            }
            else if (targetMaterial == Materials.stone)
            {
                return m_metalToStoneSound;
            }
            else if (targetMaterial == Materials.metall)
            {
                return m_metalToMetalSound;
            }
            else
            {
                return m_defaultMetalSound;
            }
        }
        else
        {
            if (targetMaterial == Materials.wood)
            {
                return m_defaultWoodSound;
            }
            else if (targetMaterial == Materials.stone)
            {
                return m_defaultStoneSound;
            }
            else if (targetMaterial == Materials.metall)
            {
                return m_defaultMetalSound;
            }
            else
            {
                return m_defaultSound;
            }
        }
        
    }
}
