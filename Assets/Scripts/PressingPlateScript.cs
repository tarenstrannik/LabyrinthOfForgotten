using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressingPlateScript : MonoBehaviour
{
    [SerializeField] private AudioSource m_plateAudioSource;
    [SerializeField] private AudioSource m_plateClickAudioSource;
    [SerializeField] private AudioClip m_movingEndedClick;


    [SerializeField] private Transform[] m_startTransforms;
    [SerializeField] private Transform[] m_endTransforms;


    [SerializeField] GameObject m_plate;


    private Coroutine m_MoveplateCoroutine = null;

    private Coroutine m_MoveplateWIthDelayCoroutine = null;
    private Coroutine m_SendCommandWIthDelayCoroutine = null;

    private float m_curTime;

    [SerializeField] private float[] m_stageMoveTime = { 3f};

    [SerializeField] private GameObject m_LinkedDoor;
    [SerializeField] private List<Collider> m_Collider;

    [SerializeField] private float  m_maxMass = 60f;
    private float m_curMass = 0f; //

    [Tooltip("The list of colliders to use for detecting weight. If not selected any, using all colliders of object and it's children")]
    private List<GameObject> m_currentColliders = new List<GameObject>();//

    private bool m_doorOpened=false;

    private float m_curPercent=0;

    private void Awake()
    {

        m_plate.transform.position = m_startTransforms[0].position;
       
    }


    public void Moveplate(float endTimePercent, float allTime)
    {
        if (m_MoveplateWIthDelayCoroutine != null)
        {
            StopCoroutine(m_MoveplateWIthDelayCoroutine);
        }
        if (m_SendCommandWIthDelayCoroutine != null)
        {
            StopCoroutine(m_SendCommandWIthDelayCoroutine);
        }
        m_MoveplateWIthDelayCoroutine =StartCoroutine(MovePlateWithDelay( endTimePercent, allTime));
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
        GameObject[] objectsToMove = { m_plate };

        if (m_MoveplateCoroutine != null)
        {
            StopMovingplateCoroutine(m_MoveplateCoroutine);
        }
        m_MoveplateCoroutine = StartCoroutine(IMoveplate(objectsToMove, endTimePercent, allTime));
        m_MoveplateWIthDelayCoroutine = null;
    }

    private void StopMovingplateCoroutine(Coroutine cor)
    {
        if (cor != null)
        {
            StopCoroutine(cor);
            m_plateAudioSource.Pause();
            m_LinkedDoor.GetComponent<DoorUpScript>().StopMovingDoorCoroutine();
        }
    }
    private IEnumerator IMoveplate(GameObject[] objectsToMove, float endTimePercent, float allTime)
    {


        Vector3 startPosition = m_startTransforms[0].position;
        Vector3 endPosition = m_endTransforms[0].position;


        int directionCoef = endTimePercent > m_curPercent ? 1 : -1;

        m_curTime = m_curPercent * allTime;

        m_plateAudioSource.Play();

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
        m_plateAudioSource.Pause();
        if (m_curPercent >= 1 || m_curPercent <= 0)
        {
            m_plateClickAudioSource.PlayOneShot(m_movingEndedClick);
        }

        if(m_curPercent >= 1)
        {
            m_doorOpened = true;
            
        }
        m_MoveplateCoroutine = null;
    }


    private void OnCollisionEnter(Collision collision)
    {
        
        if (m_Collider.Contains(collision.GetContact(0).thisCollider) || m_Collider.Count==0)
        {
            
            if (!m_doorOpened)
            {
                var colRb = collision.gameObject.GetComponent<Rigidbody>();
                if (!m_currentColliders.Contains(collision.gameObject) && colRb != null)
                {
                    m_currentColliders.Add(collision.gameObject);

                    m_curMass += colRb.mass;
                    var targetMass = m_curMass > m_maxMass ? 1 : m_curMass / m_maxMass;
                    Moveplate(targetMass, m_stageMoveTime[0]);


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
       
        if (m_currentColliders.Contains(other.gameObject))
        {
            m_currentColliders.Remove(other.gameObject);
            if (other.gameObject.GetComponent<Rigidbody>() != null)
            {
                m_curMass -= other.gameObject.GetComponent<Rigidbody>().mass;
                if (m_curMass < 0) m_curMass = 0;
            }
            
            if (m_curMass < m_maxMass && m_doorOpened)
            {
                m_doorOpened = false;
                m_plateClickAudioSource.PlayOneShot(m_movingEndedClick);

                m_LinkedDoor.GetComponent<DoorUpScript>().MoveDoorSecondStageDown();
            }
            var targetMass = m_curMass > m_maxMass ? 1 : m_curMass / m_maxMass;


            Moveplate(targetMass, m_stageMoveTime[0]);
        }
    }
}
