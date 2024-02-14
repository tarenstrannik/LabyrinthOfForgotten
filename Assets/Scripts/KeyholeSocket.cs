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
    public Transform AttachedPosition
    {
        get
        {
            return m_attachedPosition;
        }
        set
        {
            m_attachedPosition = AttachedPosition;
        }
    }
    [SerializeField] private Transform _testPosition;
    [SerializeField] private float _testDistance;
    [SerializeField] private float _testDot;
    [SerializeField] private float _testAngle;

    [SerializeField] private float m_minDotToBeAlignedForward = 0.96f;
    [SerializeField] private float m_eulerAngleToBeAlignedRotation = 15f;
    [SerializeField] private float m_distanceToRemoveKey = 0.07f;
    [SerializeField] private float m_distanceToStartRotating = 0.01f;
    [SerializeField] private float m_eulerAngleTillNoRemoveKey = 15f;


    [SerializeField] private float m_minDistanceToSocket = 0.1f;
    

    public GameObject Key { get; private set; }

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
        Func<bool> KeyIsInsertingAlignedWithHole() => () => Key!=null 
                                                            && Vector3.Dot(Key.transform.forward, AttachedPosition.transform.forward) >= m_minDotToBeAlignedForward
                                                            && Vector3.Angle(Key.transform.right, AttachedPosition.transform.right) < m_eulerAngleToBeAlignedRotation
                                                            && Vector3.Distance(Key.transform.position, AttachedPosition.transform.position) < m_distanceToRemoveKey;
        Func<bool> KeyIsRemoved() => () => Vector3.Distance(Key.transform.position, AttachedPosition.transform.position)>= m_distanceToRemoveKey;

        Func<bool> KeyIsInsideEnough() => () => Vector3.Distance(Key.transform.position, AttachedPosition.transform.position) <= m_distanceToStartRotating;
        Func<bool> KeyIsOutEnough() => () => Vector3.Distance(Key.transform.position, AttachedPosition.transform.position) > m_distanceToStartRotating;

        Func<bool> KeyIsInsideAndRotatedCWABit() => () => Vector3.Angle(Key.transform.right, AttachedPosition.transform.right) >= m_eulerAngleTillNoRemoveKey;
        Func<bool> KeyIsInsideAndRotatedCCWTillStop() => () => Vector3.Angle(Key.transform.right, AttachedPosition.transform.right) < m_eulerAngleTillNoRemoveKey;

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

        if(Key==null)
        {
            Key = args.interactableObject.transform.gameObject;
            m_keyGrabInteractable = Key.GetComponent<XRGrabInteractable>();

        }

    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if(Key== args.interactableObject.transform.gameObject)
        {
            Key = null;
        }
        //CancelInteractionWithSocket();

    }
    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            //UpdateCurrentController();
            if (Key != null)
            {
                _testDistance = Vector3.Distance(Key.transform.position, AttachedPosition.transform.position);
                _testDot = Vector3.Dot(Key.transform.forward, AttachedPosition.transform.forward);
                _testAngle = Vector3.Angle(Key.transform.right, AttachedPosition.transform.right);
                //Debug.Log("dot "+ Vector3.Dot(Key.transform.forward, AttachedPosition.transform.forward) + "dist "+Vector3.Distance(Key.transform.position, AttachedPosition.transform.position));
            }
            //CheckDistanceToHand();
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
        Key.transform.position = m_attachedPosition.position;
        Key.transform.rotation = m_attachedPosition.rotation;

        Key.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;

    }
    private void CheckDistanceToHand()
    {
        if (m_currentController != null && Vector3.Distance(Key.transform.position, m_currentController.transform.position) > m_minDistanceToSocket)
        {
            m_keyGrabInteractable.trackPosition = true;
        }
    }
    private void CancelInteractionWithSocket()
    {
        if (m_keyInKeyHole)
        {
            m_keyGrabInteractable.trackPosition = true;


            Key = null;
            m_keyGrabInteractable = null;
            m_keyInKeyHole = false;
            m_currentController = null;
            

            Key.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }


}
