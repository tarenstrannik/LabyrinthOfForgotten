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

    private StateMachine m_keyholeStateMachine;
    private KeyholeStateKeyIsInsertedAndCanRotate m_keyIsInsertedAndCanRotateState;

    private Rigidbody m_keyholeRigidbody;

    [Header("KeyHole options")]
    [SerializeField] private Transform m_insertedPositionTransform;
    [SerializeField] private Collider[] m_keyholeCollidersToNotCollideWithKeyWhenBeginInserting;

    [SerializeField] private float m_minDotToBeAlignedForwardToLetInsertKey = 0.96f;
    [SerializeField] private float m_eulerAngleToBeAlignedRotationToLetInsertKey = 15f;
    [SerializeField] private float m_distanceFromInsertedPositionToRemoveKeyFromKeyhole = 0.13f;
    [SerializeField] private float m_distanceFromInsertedPositionToLetStartRotating = 0.005f;
    [SerializeField] private float m_distanceDeltaPercentForLinearTransitions = 1f;
    [SerializeField] private float m_eulerAngleRotationTillNoRemoveKey = 15f;

    [SerializeField] private float m_eulerAngleRotationTillRotationLimit = 90f;
    [SerializeField] private float m_eulerAngleRotationTillActivation = 85f;

    [SerializeField] private float m_minDistanceFromHandToKeyToUnselect = 0.1f;

    private XRGrabInteractableWithExternallyChangeableRBParams m_keyGrabInteractable;
    public Transform KeyTransform { get; private set; }
    public Rigidbody KeyRigidbody { get; private set; }

    private Collider[] m_keyColliders;
    private Collider[] m_keyCollidersEmpty = new Collider[] { };

    //dragparameters
    [SerializeField] private bool m_keyInKeyholeIsKinematic = false;
    [SerializeField] private bool m_keyInKeyholeUseGravity = false;
    [SerializeField] private float m_keyInKeyholeDrag = 1f;
    [SerializeField] private float m_keyInKeyholeAngularDrag = 1f;


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
        Quaternion attachedPositionRotation = m_insertedPositionTransform.rotation;
        Vector3 attachedPositionRight = m_insertedPositionTransform.right;

        var keyIsInsertedAndCanMoveState = new KeyholeStateKeyIsInsertedAndCanMove(this, m_insertedPositionTransform, attachedPositionRotation,
            m_keyholeRigidbody, m_distanceFromInsertedPositionToRemoveKeyFromKeyhole, m_distanceDeltaPercentForLinearTransitions);
        var keyIsInsertedAndCanMoveRotateIntermediateState = new KeyholeStateKeyIsInsertedAndCanMoveRotate(this, attachedPositionRotation, 
            m_keyholeRigidbody, m_distanceFromInsertedPositionToLetStartRotating, m_distanceDeltaPercentForLinearTransitions, m_eulerAngleRotationTillRotationLimit);
        m_keyIsInsertedAndCanRotateState = new KeyholeStateKeyIsInsertedAndCanRotate(this, m_insertedPositionTransform.position, attachedPositionRight, 
            attachedPositionRotation, m_keyholeRigidbody, m_eulerAngleRotationTillRotationLimit, m_eulerAngleRotationTillActivation);

        //adding transitions
        //local functions for simplicity
        void AT(IState from, IState to, Func<bool> condition) => m_keyholeStateMachine.AddFromDefinedStateTransition(from, to, condition);

        AT(keyIsOutState, keyIsInsertedAndCanMoveState, KeyIsInsertingAlignedWithHole());
        AT(keyIsInsertedAndCanMoveState, keyIsOutState, KeyIsRemoved());

        AT(keyIsInsertedAndCanMoveState, keyIsInsertedAndCanMoveRotateIntermediateState, KeyIsInsideEnough());
        AT(keyIsInsertedAndCanMoveRotateIntermediateState, keyIsInsertedAndCanMoveState, KeyIsOutEnough());

        AT(keyIsInsertedAndCanMoveRotateIntermediateState, m_keyIsInsertedAndCanRotateState, KeyIsInsideAndRotatedCWABit());
        AT(m_keyIsInsertedAndCanRotateState, keyIsInsertedAndCanMoveRotateIntermediateState, KeyIsInsideAndRotatedCCWTillStop());

        //conditions
        Func<bool> KeyIsInsertingAlignedWithHole() => () => m_keyGrabInteractable != null 
                                                            && Vector3.Dot(KeyTransform.forward, m_insertedPositionTransform.forward) >= m_minDotToBeAlignedForwardToLetInsertKey
                                                            && Vector3.Angle(KeyTransform.right, m_insertedPositionTransform.right) < m_eulerAngleToBeAlignedRotationToLetInsertKey
                                                            && Vector3.Distance(m_insertedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero) < m_distanceFromInsertedPositionToRemoveKeyFromKeyhole;
        Func<bool> KeyIsRemoved() => () => Vector3.Distance(m_insertedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero) >= m_distanceFromInsertedPositionToRemoveKeyFromKeyhole;

        Func<bool> KeyIsInsideEnough() => () => Vector3.Distance(m_insertedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero) <= m_distanceFromInsertedPositionToLetStartRotating;
        Func<bool> KeyIsOutEnough() => () => Vector3.Distance(m_insertedPositionTransform.InverseTransformPoint(KeyTransform.position), Vector3.zero) > m_distanceFromInsertedPositionToLetStartRotating;

        Func<bool> KeyIsInsideAndRotatedCWABit() => () => Vector3.Angle(KeyTransform.right, attachedPositionRight) >= m_eulerAngleRotationTillNoRemoveKey;
        Func<bool> KeyIsInsideAndRotatedCCWTillStop() => () => Vector3.Angle(KeyTransform.right, attachedPositionRight) < m_eulerAngleRotationTillNoRemoveKey;

        // setting start state
        m_keyholeStateMachine.SetState(keyIsOutState);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        m_keyIsInsertedAndCanRotateState.OnMaxReached += MaxRotationReached;
        m_keyIsInsertedAndCanRotateState.OnMaxLeft += MaxRotationLeft;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        m_keyIsInsertedAndCanRotateState.OnMaxReached -= MaxRotationReached;
        m_keyIsInsertedAndCanRotateState.OnMaxLeft -= MaxRotationLeft;
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
            m_keyGrabInteractable = key.GetComponent<XRGrabInteractableWithExternallyChangeableRBParams>();
            m_keyColliders = key.GetComponentsInChildren<Collider>();
            KeyTransform = key.transform;
            KeyRigidbody = key.GetComponent<Rigidbody>();
            SaveKeyRigidbodyParameters();
        }

    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {

        base.OnHoverExited(args);
        if(m_keyGrabInteractable == args.interactableObject.transform.gameObject.GetComponent<XRGrabInteractableWithExternallyChangeableRBParams>())
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
            } 
        }
    }

    private void CheckIfKeyInHandAndDistanceToHand()
    {

        if (m_keyGrabInteractable.interactorsSelecting.Count > 0)
        {
            var controllerInteractor = m_keyGrabInteractable.interactorsSelecting[0] as XRBaseInteractor;

            if (controllerInteractor != null && Vector3.Distance(KeyTransform.position, controllerInteractor.transform.position) > m_minDistanceFromHandToKeyToUnselect)
            {
                // unsubscribe hand from key
                var interactorManager = controllerInteractor.interactionManager;
                var iSelectInteractor = m_keyGrabInteractable.interactorsSelecting[0] as IXRSelectInteractor;
                interactorManager.CancelInteractorSelection(iSelectInteractor);
                
            }

        }
    }

    private void SetCollisionBetweenKeyAndKeyholeDisabled(bool value)
    {
        foreach (var keyholeCollider in m_keyholeCollidersToNotCollideWithKeyWhenBeginInserting)
        {
            foreach (var keyCollider in m_keyColliders)
            {
                Physics.IgnoreCollision(keyholeCollider, keyCollider, value);
            }
        }
    }

    private void SaveKeyRigidbodyParameters()
    {
        if (m_keyGrabInteractable.interactorsSelecting.Count == 0)
        {
            m_keyGrabInteractable.SaveDefaultRBParams(KeyRigidbody.isKinematic, KeyRigidbody.useGravity, KeyRigidbody.drag, KeyRigidbody.angularDrag);
        }
        m_keyGrabInteractable.SaveManualRBParams(m_keyInKeyholeIsKinematic, m_keyInKeyholeUseGravity, m_keyInKeyholeDrag, m_keyInKeyholeAngularDrag);
 
    }

    public void SetKeyInKeyhole(bool value)
    {
        SetCollisionBetweenKeyAndKeyholeDisabled(value);
        m_keyGrabInteractable?.SetManualChangedStateActive(value);
    }

    private void MaxRotationReached()
    {

        m_onMaxReached.Invoke();
    }
    private void MaxRotationLeft()
    {

        m_onMaxLeft.Invoke();
    }
    public void ClearInternalMaxReachedLeftlEventsSubscription()
    {
        m_keyIsInsertedAndCanRotateState.OnMaxReached -= MaxRotationReached;
        m_keyIsInsertedAndCanRotateState.OnMaxLeft -= MaxRotationLeft;
    }
}
