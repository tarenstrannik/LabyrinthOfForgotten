using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSelectionController : MonoBehaviour, IObserveAllPlayerSelections
{
    [SerializeField] private XRBaseInteractor[] m_interactorsToMonitor;

    [SerializeField] UnityEvent<SelectEnterEventArgs> m_onByPlayerSelection;
    public UnityEvent<SelectEnterEventArgs> OnByPlayerSelection 
    {
        get
        {
            return m_onByPlayerSelection;
        } 
    }
    [SerializeField] UnityEvent<SelectExitEventArgs> m_onByPlayerDeselection;
    public UnityEvent<SelectExitEventArgs> OnByPlayerDeselection
    {
        get
        {
            return m_onByPlayerDeselection;
        } 
    }
    private void Awake()
    {
        m_interactorsToMonitor = GetComponentsInChildren<XRBaseInteractor>();

    }

    private void OnEnable()
    {
        foreach (XRBaseInteractor interactor in m_interactorsToMonitor)
        {
            interactor.selectEntered.AddListener(ProcessSelection);
            interactor.selectExited.AddListener(ProcessDeselection);
        }
    }
    private void OnDisable()
    {
        foreach (XRBaseInteractor interactor in m_interactorsToMonitor)
        {
            interactor.selectEntered.RemoveListener(ProcessSelection);
            interactor.selectExited.RemoveListener(ProcessDeselection);
        }
    }


    public void ProcessSelection(SelectEnterEventArgs args)
    {
        OnByPlayerSelection.Invoke(args);
    }
    public void ProcessDeselection(SelectExitEventArgs args)
    {
        OnByPlayerDeselection.Invoke(args);

    }

}
