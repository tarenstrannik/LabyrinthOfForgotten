using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressingPlateScript : MonoBehaviour, IMoveLinearToTargetPercent, IHaveMinMax
{
    //moving options
    [SerializeField] private Transform[] m_startTransforms;
    [SerializeField] private Transform[] m_endTransforms;

    //what to move
    [Tooltip("This transform would be moved")]
    [SerializeField] private Transform m_plate;

    //Collision and weight
    [Header("Collision and weight options")]

    [Tooltip("Object with component to track collisions")]
    [SerializeField] private ICollisionDetector m_collisionDetector;

    [Tooltip("Colliders on it and its child used to detect Enter Collision Event (if none any child would be used)")]
    [SerializeField] private List<Collider> m_plateColliders;
    private Dictionary<GameObject,float> m_currentObjectsOnPlate = new Dictionary<GameObject, float>();

    [Tooltip("Make it a bit less than you really need because of float comparision")]
    [SerializeField] private float m_activationMass = 60f;
    private float m_curMass = 0f; 


    // Events
    [Header("Events options")]

    [SerializeField] private UnityEvent m_activatedEvent;
    public UnityEvent OnMaxReachedEvent
    {
        get
        {
            return m_activatedEvent;
        }

    }
    [SerializeField] private UnityEvent m_deactivatedEvent;
    public UnityEvent OnMaxLeftEvent
    {
        get
        {
            return m_deactivatedEvent;
        }

    }

    [SerializeField] private UnityEvent<float> m_positionChangeBeginEvent;
    public UnityEvent<float> OnPositionChangeToTargetPercentBegin
    {
        get
        {
            return m_positionChangeBeginEvent;
        }

    }
    [SerializeField] private UnityEvent m_positionChangingEndEvent;
    public UnityEvent OnPositionChangeEnd
    {
        get
        {
            return m_positionChangingEndEvent;
        }
    }

    [SerializeField] private UnityEvent m_startPositionReachedEvent;
    public UnityEvent OnMinReachedEvent 
    { 
        get
        {
            return m_startPositionReachedEvent;
        }
    }
    public UnityEvent OnMinLeftEvent { get; }

    //Coroutines

    private Coroutine m_MoveplateCoroutine = null;

    [Header("Moving stages options")]
    [SerializeField] private float m_moveFullCycleTime = 3f;

    [SerializeField] private GameObject m_LinkedDoor;

    private bool m_plateActivated=false;

    private float m_curPercent=0;


    private void OnEnable()
    {
        OnPositionChangeToTargetPercentBegin.AddListener(MovePlate);
        m_collisionDetector.OnCollisionEnterDetection += OnCollisionEnter;
        m_collisionDetector.OnTriggerExitDetection += OnTriggerExit;

    }
    private void OnDisable()
    {
        
        OnPositionChangeToTargetPercentBegin.RemoveListener(MovePlate);
        m_collisionDetector.OnCollisionEnterDetection -= OnCollisionEnter;
        m_collisionDetector.OnTriggerExitDetection -= OnTriggerExit;
    }

    private void Awake()
    {
        if (m_collisionDetector == null) m_collisionDetector = GetComponentInChildren<ICollisionDetector>();
        m_plate.position = m_startTransforms[0].position;
    }

    private void MovePlate(float endTimePercent)
    {

        if (m_MoveplateCoroutine != null)
        {
            StopCoroutine(m_MoveplateCoroutine);
        }
        Transform[] objectsToMove = { m_plate };
        m_MoveplateCoroutine = StartCoroutine(IMoveplate(objectsToMove, endTimePercent));

    }
    private IEnumerator IMoveplate(Transform[] objectsToMove, float endTimePercent)
    {


        Vector3 startPosition = m_startTransforms[0].position;
        Vector3 endPosition = m_endTransforms[0].position;


        int directionCoef = endTimePercent > m_curPercent ? 1 : -1;

        while (directionCoef * m_curPercent  <= directionCoef * endTimePercent )
        {
            foreach (var obj in objectsToMove)
            {

                obj.transform.position = Vector3.Lerp(startPosition, endPosition, m_curPercent);
            }
            m_curPercent += directionCoef * Time.deltaTime/ m_moveFullCycleTime;
            yield return null;
        }
        m_curPercent = endTimePercent;
        m_curPercent = m_curPercent > 1 ? 1 : m_curPercent;
        m_curPercent = m_curPercent < 0 ? 0 : m_curPercent;

        OnPositionChangeEnd.Invoke();
        if (m_curPercent <= 0)
        {
            OnMinReachedEvent.Invoke();
        }

        if(m_curPercent >= 1)
        {
            m_plateActivated = true;
            OnMaxReachedEvent.Invoke();
        }
        m_MoveplateCoroutine = null;
    }


    private void OnCollisionEnter(Collision collision)
    {

        if (m_plateColliders.Count == 0 || m_plateColliders.Contains(collision.GetContact(0).thisCollider))
        {
            var colRb = collision.gameObject.GetComponent<Rigidbody>();
            if (colRb != null && m_currentObjectsOnPlate.TryAdd(collision.gameObject, colRb.mass))
            {
                m_curMass += colRb.mass;
                if (!m_plateActivated)
                {
                    var targetPercent = m_curMass >= m_activationMass ? 1 : m_curMass / m_activationMass;
                    //MovePlate(targetMass, m_moveFullCycleTime);
                    OnPositionChangeToTargetPercentBegin.Invoke(targetPercent);
                    
                }


            }

        }
     }

    private void OnTriggerExit(Collider other)
    {

        if (m_currentObjectsOnPlate.TryGetValue(other.gameObject, out var value))
        {

            m_currentObjectsOnPlate.Remove(other.gameObject);
            m_curMass -= value;
            if (m_curMass < 0) m_curMass = 0;

            
            if (m_curMass < m_activationMass)
            {
                if (m_plateActivated)
                {
                    m_plateActivated = false;
                    OnMaxLeftEvent.Invoke();
                }
                var targetPercent = m_curMass > m_activationMass ? 1 : m_curMass / m_activationMass;
                //MovePlate(targetMass, m_moveFullCycleTime);
                OnPositionChangeToTargetPercentBegin.Invoke(targetPercent);
            }

           
        }
    }
}
