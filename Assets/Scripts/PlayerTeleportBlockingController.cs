using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerTeleportBlockingController : MonoBehaviour, ISwitchTeleportBlockNeed
{
    [SerializeField] private UnityEvent m_onTeleportBlock;
    public UnityEvent OnTeleportBlock 
    {
        get
        {
            return m_onTeleportBlock;
        }
    }
    [SerializeField] private UnityEvent m_onTeleportUnblock;
    public UnityEvent OnTeleportUnblock 
    {
        get
        {
            return m_onTeleportUnblock;
        }
    }
    private bool m_isTeleportBlocked = false;
    public bool IsTeleportBlocked 
    {
        get
        {
            return m_isTeleportBlocked;
        }
        private set
        {
            m_isTeleportBlocked = value;
        }
    }

    [SerializeField] private IObserveAllPlayerSelections m_allPlayerSelectionsObserver;
    private List<GameObject> m_playerTPBlockingObejctsList = new List<GameObject>();

    private void Awake()
    {
        m_allPlayerSelectionsObserver=GetComponentInChildren<IObserveAllPlayerSelections>();
    }

    private void OnEnable()
    {

        m_allPlayerSelectionsObserver.OnByPlayerSelection.AddListener(ProcessTPBlockingObjectOnSelect);
        m_allPlayerSelectionsObserver.OnByPlayerDeselection.AddListener(ProcessTPBlockingObjectOnDeselect);

    }
    private void OnDisable()
    {

        m_allPlayerSelectionsObserver.OnByPlayerSelection.RemoveListener(ProcessTPBlockingObjectOnSelect);
        m_allPlayerSelectionsObserver.OnByPlayerDeselection.RemoveListener(ProcessTPBlockingObjectOnDeselect);

    }

    public void ProcessTPBlockingObjectOnSelect(SelectEnterEventArgs args)
    {
        var blockingObject = args.interactableObject.transform.GetComponentInChildren<ICanBlockTeleportIfActiveAndInHand>();
        if (blockingObject != null)
        {
            blockingObject.OnActivated.AddListener(AddTPBlockingObjectInListOnActivation);
            blockingObject.OnDeactivated.AddListener(RemoveTPBlockingObjectFromListOnDeactivation);
            if (blockingObject.IsActive)
            {
                AddTPBlockingObjectInListOnActivation(args.interactableObject.transform.gameObject);
            }
        }
    }
    public void ProcessTPBlockingObjectOnDeselect(SelectExitEventArgs args)
    {
        var blockingObject = args.interactableObject.transform.GetComponentInChildren<ICanBlockTeleportIfActiveAndInHand>();
        if (blockingObject != null)
        {
            blockingObject.OnActivated.RemoveListener(AddTPBlockingObjectInListOnActivation);
            blockingObject.OnDeactivated.RemoveListener(RemoveTPBlockingObjectFromListOnDeactivation);
            if (blockingObject.IsActive)
            {
                RemoveTPBlockingObjectFromListOnDeactivation(args.interactableObject.transform.gameObject);
            }
        }

    }
    private void AddTPBlockingObjectInListOnActivation(GameObject obj)
    {

        m_playerTPBlockingObejctsList.Add(obj);
        if(!IsTeleportBlocked && m_playerTPBlockingObejctsList.Count > 0)
        {
            IsTeleportBlocked = true;
            OnTeleportBlock.Invoke();
            
        }

    }
    private void RemoveTPBlockingObjectFromListOnDeactivation(GameObject obj)
    {
        m_playerTPBlockingObejctsList.Remove(obj);
        if (IsTeleportBlocked && m_playerTPBlockingObejctsList.Count == 0)
        {
            IsTeleportBlocked = false;
            OnTeleportUnblock.Invoke();
        }
    }
   

}
