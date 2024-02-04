using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerObjectsReactingOnByPlayerSelectionController : MonoBehaviour
{
    [SerializeField] private IObserveAllPlayerSelections m_allPlayerSelectionsObserver;
    private void Awake()
    {
        m_allPlayerSelectionsObserver = GetComponentInChildren<IObserveAllPlayerSelections>();

    }

    private void OnEnable()
    {
        m_allPlayerSelectionsObserver.OnByPlayerSelection.AddListener(ProcessSelectionByPlayer);
        m_allPlayerSelectionsObserver.OnByPlayerDeselection.AddListener(ProcessDeselectionByPlayer);

    }
    private void OnDisable()
    {

        m_allPlayerSelectionsObserver.OnByPlayerSelection.RemoveListener(ProcessSelectionByPlayer);
        m_allPlayerSelectionsObserver.OnByPlayerDeselection.RemoveListener(ProcessDeselectionByPlayer);

    }

    public void ProcessSelectionByPlayer(SelectEnterEventArgs args)
    {
        IReactOnSelectionByPlayer[] observers = args.interactableObject.transform.GetComponentsInChildren<IReactOnSelectionByPlayer>();
        foreach(IReactOnSelectionByPlayer observer in observers)
        {
            observer.OnByPlayerSelected.Invoke(args);
        }
    }
    public void ProcessDeselectionByPlayer(SelectExitEventArgs args)
    {
        IReactOnSelectionByPlayer[] observers = args.interactableObject.transform.GetComponentsInChildren<IReactOnSelectionByPlayer>();
        foreach (IReactOnSelectionByPlayer observer in observers)
        {
            observer.OnByPlayerDeselected.Invoke(args);
        }
    }



}
