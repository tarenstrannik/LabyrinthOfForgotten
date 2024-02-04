using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleSimulationOnPlayerSelect : MonoBehaviour, IReactOnSelectionByPlayer
{
    [SerializeField] private ParticleSystem m_fire;

    private ParticleSystemCullingMode m_startCullingMode;

    [SerializeField] private UnityEvent<SelectEnterEventArgs> m_onByPlayerSelected;
    public UnityEvent<SelectEnterEventArgs> OnByPlayerSelected 
    { 
        get
        {
            return m_onByPlayerSelected;
        }
    }
    [SerializeField] private UnityEvent<SelectExitEventArgs> m_onByPlayerDeselected;
    public UnityEvent<SelectExitEventArgs> OnByPlayerDeselected 
    { 
        get
        {
            return m_onByPlayerDeselected;
        }
    }

    void Awake()
    {
        m_startCullingMode = m_fire.main.cullingMode;
    }

    private void OnEnable()
    {
        OnByPlayerSelected.AddListener(ChangeCullingMode);
        OnByPlayerDeselected.AddListener(RestoreCullingMode);
    }
    private void OnDisable()
    {
        OnByPlayerSelected.RemoveListener(ChangeCullingMode);
        OnByPlayerDeselected.RemoveListener(RestoreCullingMode);
    }
    public void ChangeCullingMode(SelectEnterEventArgs args)
    {

            var main = m_fire.main;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;

    }
    public void RestoreCullingMode(SelectExitEventArgs args)
    {
        var main = m_fire.main;
        main.cullingMode = m_startCullingMode;
    }
}
