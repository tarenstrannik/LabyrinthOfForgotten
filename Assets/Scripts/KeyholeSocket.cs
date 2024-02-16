using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class KeyholeSocket : XRSocketInteractor, IHaveMinMax
{
    [Header("KeyHole options")]
    private StateMachine m_keyholeStateMachine;
    private Rigidbody m_keyholeRigidbody;
    [SerializeField] private Transform m_attachedPositionTransform;
    [SerializeField] private Collider[] m_keyholeCollidersToNotCollideWithKey;


    [SerializeField] private float _testDistance;
    [SerializeField] private float _testDot;
    [SerializeField] private float _testAngle;

    [SerializeField] private float m_minDotToBeAlignedForward = 0.96f;
    [SerializeField] private float m_eulerAngleToBeAlignedRotation = 15f;
    [SerializeField] private float m_distanceToRemoveKey = 0.13f;
    [SerializeField] private float m_distanceToStartRotating = 0.005f;
    [SerializeField] private float m_eulerAngleTillNoRemoveKey = 15f;


    [SerializeField] private float m_minDistanceToKeyToUnselect = 0.1f;

    private XRGrabInteractable m_keyGrabInteractable;
    public Transform KeyTransform { get; private set; }
    public Rigidbody KeyRigidbody { get; private set; }

    private Collider[] m_keyColliders;
    private Collider[] m_keyCollidersEmpty = new Collider[] { };
    //dragparameters
    [SerializeField] private float m_keyInKeyholeDrag = 1f;
    [SerializeField] private float m_keyInKeyholeAngularDrag = 1f;
    private float m_keySavedDrag;
    private float m_keySavedAngularDrag;

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

        m_keyColliders = m_keyCollidersEmpty;
        m_keyholeRigidbody=GetComponentInChildren<Rigidbody>();

        m_keyholeStateMachine = new StateMachine();
        // init states
        var keyIsOutState = new KeyholeStateKeyIsOut(this);
        var keyIsInsertedAndCanMoveState = new KeyholeStateKeyIsInsertedAndCanMove(this, m_attachedPositionTransform, m_attachedPositionTransform.rotation, m_keyholeRigidbody, m_distanceToRemoveKey);
        var keyIsInsertedAndCanMoveRotateIntermediateState = new KeyholeStateKeyIsInsertedAndCanMoveRotate(this, m_attachedPositionTransform, m_keyholeRigidbody, m_distanceToStartRotating);
        var keyIsInsertedAndCanRotateState = new KeyholeStateKeyIsInsertedAndCanRotate(this, m_attachedPositionTransform, m_attachedPositionTransform.position, m_keyholeRigidbody);

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
        Func<bool> KeyIsInsertingAlignedWithHole() => () => m_keyGrabInteractable != null 
                                                            && Vector3.Dot(KeyTransform.forward, m_attachedPositionTransform.forward) >= m_minDotToBeAlignedForward
                                                            && Vector3.Angle(KeyTransform.right, m_attachedPositionTransform.right) < m_eulerAngleToBeAlignedRotation
                                                            && Vector3.Distance(m_attachedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero) < m_distanceToRemoveKey;
        Func<bool> KeyIsRemoved() => () => Vector3.Distance(m_attachedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero) >= m_distanceToRemoveKey;

        Func<bool> KeyIsInsideEnough() => () => Vector3.Distance(m_attachedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero) <= m_distanceToStartRotating;
        Func<bool> KeyIsOutEnough() => () => Vector3.Distance(m_attachedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero) > m_distanceToStartRotating;

        Func<bool> KeyIsInsideAndRotatedCWABit() => () => Vector3.Angle(KeyTransform.right, m_attachedPositionTransform.right) >= m_eulerAngleTillNoRemoveKey;
        Func<bool> KeyIsInsideAndRotatedCCWTillStop() => () => Vector3.Angle(KeyTransform.right, m_attachedPositionTransform.right) < m_eulerAngleTillNoRemoveKey;

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

        if(m_keyGrabInteractable==null)
        {
            var key = args.interactableObject.transform.gameObject;
            m_keyGrabInteractable = key.GetComponent<XRGrabInteractable>();
            m_keyColliders = key.GetComponentsInChildren<Collider>();
            KeyTransform = key.transform;
            KeyRigidbody = key.GetComponent<Rigidbody>();
            SaveKeyDragParameters();
        }

    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if(m_keyGrabInteractable == args.interactableObject.transform.gameObject.GetComponent<XRGrabInteractable>())
        {
            m_keyGrabInteractable = null;
            m_keyColliders = m_keyCollidersEmpty;
            KeyTransform = null;
            KeyRigidbody = null;
        }

    }
    public override void ProcessInteractor(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractor(updatePhase);

        if (m_keyGrabInteractable != null)
        {
            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Fixed)
            {
                CheckIfKeyInHandAndDistanceToHand();
                m_keyholeStateMachine.Process();
                _testDistance = Vector3.Distance(m_attachedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero);
                _testDot = Vector3.Dot(KeyTransform.forward, m_attachedPositionTransform.forward);
                _testAngle = Vector3.Angle(KeyTransform.right, m_attachedPositionTransform.right);
                
            } 
        }
    }

    private void CheckIfKeyInHandAndDistanceToHand()
    {

        if (m_keyGrabInteractable.interactorsSelecting.Count > 0)
        {
            var controllerInteractor = m_keyGrabInteractable.interactorsSelecting[0] as XRBaseControllerInteractor;
            if (controllerInteractor != null && Vector3.Distance(KeyTransform.position, controllerInteractor.transform.position) > m_minDistanceToKeyToUnselect)
            {
                // unsubscribe hand from key
                controllerInteractor.allowSelect = false;
                controllerInteractor.allowSelect = true;
            }

        }
    }

    public void SetCollisionBetweenKeyAndKeyholeActive(bool value)
    {
        foreach (var keyholeCollider in m_keyholeCollidersToNotCollideWithKey)
        {
            foreach (var keyCollider in m_keyColliders)
            {
                Physics.IgnoreCollision(keyholeCollider, keyCollider, value);
            }
        }
    }

    private void SaveKeyDragParameters()
    {
        m_keySavedDrag = KeyRigidbody.drag;
        m_keySavedDrag = KeyRigidbody.angularDrag;
    }
    public void SetDragParametersKeyInKeyhole(bool value)
    {
        if (KeyRigidbody != null)
        {
            if (value)
            {
                KeyRigidbody.drag = m_keyInKeyholeDrag;
                KeyRigidbody.angularDrag = m_keyInKeyholeAngularDrag;
            }
            else
            {
                KeyRigidbody.drag = m_keySavedDrag;
                KeyRigidbody.angularDrag = m_keySavedAngularDrag;

            }
        }
    }

}
