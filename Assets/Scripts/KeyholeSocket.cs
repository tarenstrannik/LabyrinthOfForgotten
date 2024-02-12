using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;


public class KeyholeSocket : XRSocketInteractor, IHaveMinMax
{
    [Header("KeyHole options")]
    private StateMachine m_keyholeStateMachine;
    [SerializeField] private Transform m_attachedPosition;


    [SerializeField] private float m_minDistanceToSocket = 0.1f;
    

    private GameObject m_key;
    private XRGrabInteractable m_keyGrabInteractable;
    private bool m_keyInKeyHole = false;
    private XRBaseController m_currentController;

    [Header("KeyHole Events")]
    [SerializeField] private UnityEvent m_onMinReached;
    public UnityEvent OnMinReachedEvent { get { return m_onMinReached; } }
    [SerializeField] private UnityEvent m_onMinLeft;
    public UnityEvent OnMinLeftEvent { get { return m_onMinLeft; } }
    [SerializeField] private UnityEvent m_onMaxReached;
    public UnityEvent OnMaxReachedEvent { get { return m_onMaxReached; } }

    [SerializeField] private UnityEvent m_onMaxLeft;
    public UnityEvent OnMaxLeftEvent { get { return m_onMaxLeft; } }
    protected override void Awake()
    {
        base.Awake();
        m_keyholeStateMachine = new StateMachine();
        // init states
        var keyIsOutState = new KeyholeStateKeyIsOut();
        var keyIsInsertedAndCanMoveState = new KeyholeStateKeyIsInsertedAndCanMove();
        var keyIsInsertedAndCanMoveRotateIntermediateState = new KeyholeStateKeyIsInsertedAndCanMoveRotate();
        var keyIsInsertedAndCanRotateState = new KeyholeStateKeyIsInsertedAndCanRotate();

        //adding transitions
        //local functions for simplicity
        void AT(IState from, IState to, Func<bool> condition) => m_keyholeStateMachine.AddFromDefinedStateTransition(from, to, condition);

        AT(keyIsOutState, keyIsInsertedAndCanMoveState, KeyIsInsertingAlignedWithHole());
        AT(keyIsInsertedAndCanMoveState, keyIsOutState, KeyIsRemoved());
        AT(keyIsInsertedAndCanMoveState, keyIsInsertedAndCanMoveRotateIntermediateState, KeyIsInsideEnough());
        AT(keyIsInsertedAndCanMoveRotateIntermediateState, keyIsInsertedAndCanMoveState, KeyIsOutEnough());
        AT(keyIsInsertedAndCanMoveRotateIntermediateState, keyIsInsertedAndCanRotateState, KeyIsInsideAndRotatedCWABit());
        AT(keyIsInsertedAndCanRotateState, keyIsInsertedAndCanMoveRotateIntermediateState, KeyIsInsideAndRotatedCCWTillStop());

        //conditions
        Func<bool> KeyIsInsertingAlignedWithHole() => () => 1 == 1;
        Func<bool> KeyIsRemoved() => () => 1 == 1;
        Func<bool> KeyIsInsideEnough() => () => 1 == 1;
        Func<bool> KeyIsOutEnough() => () => 1 == 1;
        Func<bool> KeyIsInsideAndRotatedCWABit() => () => 1 == 1;
        Func<bool> KeyIsInsideAndRotatedCCWTillStop() => () => 1 == 1;

        // setting start state
        m_keyholeStateMachine.SetState(keyIsOutState);
    }


    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {

    }
    protected override void OnSelectExited(SelectExitEventArgs args)
    {

    }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return false;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);

        if(CheckKeyAligned())
        {
            m_key = args.interactableObject.transform.gameObject;
            m_keyGrabInteractable = m_key.GetComponent<XRGrabInteractable>();
            m_keyInKeyHole = true;
            InsertKeyInKeyHole();
        }

    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        CancelInteractionWithSocket();

    }
    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            UpdateCurrentController();

            CheckDistanceToHand();
            m_keyholeStateMachine.Process();
        }

    }
    private bool CheckKeyAligned()
    {
        return true;
    }
    private void UpdateCurrentController()
    {
        if (m_keyGrabInteractable.interactorsSelecting.Count > 0)
        {
            var controllerInteractor = m_keyGrabInteractable.interactorsSelecting[0] as XRBaseControllerInteractor;
            if (controllerInteractor != null) m_currentController = controllerInteractor.xrController;
        }
        else
        {
            m_currentController = null;
        }
    }
    private void InsertKeyInKeyHole()
    {

        m_keyGrabInteractable.trackPosition = false;
        
        // Debug.Log(m_cardGrabInteractable.trackPosition);
        m_key.transform.position = m_attachedPosition.position;
        m_key.transform.rotation = m_attachedPosition.rotation;

        m_key.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

    }
    private void CheckDistanceToHand()
    {
        if (m_currentController != null && Vector3.Distance(m_key.transform.position, m_currentController.transform.position) > m_minDistanceToSocket)
        {
            m_keyGrabInteractable.trackPosition = true;
        }
    }
    private void CancelInteractionWithSocket()
    {
        if (m_keyInKeyHole)
        {
            m_keyGrabInteractable.trackPosition = true;


            m_key = null;
            m_keyGrabInteractable = null;
            m_keyInKeyHole = false;
            m_currentController = null;
            

            m_key.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }


}
