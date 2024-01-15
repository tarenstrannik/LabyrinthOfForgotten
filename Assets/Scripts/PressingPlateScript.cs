using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressingPlateScript : MonoBehaviour, IMoveLinear, IHaveMinMax
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

    [SerializeField] private UnityEvent<float,float> m_positionChangeBeginEvent;
    public UnityEvent<float, float> OnPositionChangeBegin
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

    private Coroutine m_MoveplateWithDelayCoroutine = null;
    private Coroutine m_SendCommandWIthDelayCoroutine = null;

    private float m_curTime;
    [Header("Moving stages options")]
    [SerializeField] private float[] m_stageMoveTime = { 3f};

    [SerializeField] private GameObject m_LinkedDoor;

    private bool m_plateActivated=false;

    private float m_curPercent=0;


    private void OnEnable()
    {
        OnPositionChangeBegin.AddListener(MovePlate);
        m_collisionDetector.OnCollisionEnterDetection += OnCollisionEnter;
        m_collisionDetector.OnTriggerExitDetection += OnTriggerExit;

    }
    private void OnDisable()
    {
        
        OnPositionChangeBegin.RemoveListener(MovePlate);
        m_collisionDetector.OnCollisionEnterDetection -= OnCollisionEnter;
        m_collisionDetector.OnTriggerExitDetection -= OnTriggerExit;
    }

    private void Awake()
    {
        if (m_collisionDetector == null) m_collisionDetector = GetComponentInChildren<ICollisionDetector>();
        m_plate.position = m_startTransforms[0].position;
    }

    private void MovePlate(float endTimePercent, float allTime)
    {

        if (m_MoveplateWithDelayCoroutine != null)
        {
            StopCoroutine(m_MoveplateWithDelayCoroutine);
        }
        if (m_SendCommandWIthDelayCoroutine != null)
        {
            StopCoroutine(m_SendCommandWIthDelayCoroutine);
        }
        m_MoveplateWithDelayCoroutine =StartCoroutine(MovePlateWithDelay( endTimePercent, allTime));
        m_SendCommandWIthDelayCoroutine=StartCoroutine(SendMoveCommandToDoorWithDelay(endTimePercent, allTime));
    }

    private IEnumerator SendMoveCommandToDoorWithDelay(float endTimePercent, float allTime)
    {
        while (m_LinkedDoor.GetComponent<DoorUpScript>().m_IsFalling)
        { 
            yield return null;
        }
        
        m_LinkedDoor.GetComponent<DoorUpScript>().MoveDoor(endTimePercent, allTime);
        m_SendCommandWIthDelayCoroutine = null;
    }

    private IEnumerator MovePlateWithDelay(float endTimePercent, float allTime)
    {

        while (m_LinkedDoor.GetComponent<DoorUpScript>().m_IsFalling)
        {
            yield return null;
        }
        Transform[] objectsToMove = { m_plate };

        if (m_MoveplateCoroutine != null)
        {
            StopMovingPlateCoroutine(m_MoveplateCoroutine);
        }
        m_MoveplateCoroutine = StartCoroutine(IMoveplate(objectsToMove, endTimePercent, allTime));
        m_MoveplateWithDelayCoroutine = null;
    }

    private void StopMovingPlateCoroutine(Coroutine cor)
    {
        if (cor != null)
        {
            StopCoroutine(cor);
            m_LinkedDoor.GetComponent<DoorUpScript>().StopMovingDoorCoroutine();
        }
    }
    private IEnumerator IMoveplate(Transform[] objectsToMove, float endTimePercent, float allTime)
    {


        Vector3 startPosition = m_startTransforms[0].position;
        Vector3 endPosition = m_endTransforms[0].position;


        int directionCoef = endTimePercent > m_curPercent ? 1 : -1;

        m_curTime = m_curPercent * allTime;



        while (directionCoef * m_curTime <= directionCoef * endTimePercent * allTime)
        {
            foreach (var obj in objectsToMove)
            {

                obj.transform.position = Vector3.Lerp(startPosition, endPosition, m_curTime / allTime);
            }
            m_curTime += directionCoef * Time.deltaTime;
            m_curPercent = m_curTime / allTime;
            yield return null;
        }
        m_curPercent = m_curTime / allTime;
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
                    var targetMass = m_curMass > m_activationMass ? 1 : m_curMass / m_activationMass;
                    //MovePlate(targetMass, m_stageMoveTime[0]);
                    OnPositionChangeBegin.Invoke(targetMass, m_stageMoveTime[0]);
                }


            }

        }
     }
    /*
     private void OnCollisionExit(Collision collision)
     {
         if(m_curMass< m_maxMass && m_doorOpened)
         {
             m_doorOpened = false;
             m_plateClickAudioSource.PlayOneShot(m_movingEndedClick);
             m_curWaitingTimeout = m_doorFallingTimeout;
             m_LinkedDoor.GetComponent<DoorUpScript>().MoveDoorSecondStageDown();
         }
         m_currentColliders.Remove(collision.gameObject);
         var prevMass = m_curMass;
         m_curMass -= collision.gameObject.GetComponent<Rigidbody>().mass;
         Moveplate(prevMass / m_maxMass, m_curMass / m_maxMass, m_stageMoveTime[0]);
     }
    */


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
                    m_LinkedDoor.GetComponent<DoorUpScript>().MoveDoorSecondStageDown();
                }
                var targetMass = m_curMass > m_activationMass ? 1 : m_curMass / m_activationMass;
                //MovePlate(targetMass, m_stageMoveTime[0]);
                OnPositionChangeBegin.Invoke(targetMass, m_stageMoveTime[0]);
            }

           
        }
    }
}
