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
    private float m_curTime;

    [SerializeField] private float[] m_stageMoveTime = { 3f};
    private float m_doorFallingTimeout = 1f;
    private float m_curWaitingTimeout = 0f;
    [SerializeField] private GameObject m_LinkedDoor;
    

    [SerializeField] private float[] m_maxMass = {60f };
    private float m_curMass = 0f;

    private List<GameObject> m_currentColliders = new List<GameObject>();

    private void Awake()
    {

        m_plate.transform.position = m_startTransforms[0].position;
        m_doorFallingTimeout = m_LinkedDoor.GetComponent<DoorUpScript>().MoveDownTimeSecondStage;
    }



    public void Moveplate(float startTimePercent, float endTimePercent, float allTime)
    {

            
            GameObject[] objectsToMove = { m_plate };
            if (m_MoveplateCoroutine != null)
            {
                StopMovingplateCoroutine(m_MoveplateCoroutine);
            }
            m_MoveplateCoroutine = StartCoroutine(IMoveplate(objectsToMove, startTimePercent, endTimePercent, allTime));
        SendMoveCommandToDoorWithDelay(m_curWaitingTimeout,startTimePercent, endTimePercent, allTime);
           
    }

    private IEnumerator SendMoveCommandToDoorWithDelay(float delay, float startTimePercent, float endTimePercent, float allTime)
    {
        yield return new WaitForSeconds(delay);
        m_LinkedDoor.GetComponent<DoorUpScript>().MoveDoor(startTimePercent, endTimePercent, allTime);
    }

    private IEnumerator DecreasingDelay()
    {
        m_curWaitingTimeout = m_doorFallingTimeout;
        while (m_curWaitingTimeout>=0)
        {
            m_curWaitingTimeout -= Time.deltaTime;
            yield return null;
        }
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
    private IEnumerator IMoveplate(GameObject[] objectsToMove, float startTimePercent, float endTimePercent, float allTime)
    {


        Vector3 startPosition = m_startTransforms[0].position;
        Vector3 endPosition = m_endTransforms[0].position;


        int directionCoef = endTimePercent > startTimePercent ? 1 : -1;

        m_curTime = startTimePercent * allTime;

        m_plateAudioSource.Play();

        while (directionCoef * m_curTime <= directionCoef * endTimePercent * allTime)
        {
            foreach (var obj in objectsToMove)
            {

                obj.transform.position = Vector3.Lerp(startPosition, endPosition, m_curTime / allTime);
            }
            m_curTime += directionCoef * Time.deltaTime;

            yield return null;
        }
        m_plateAudioSource.Pause();
        if (m_curTime / allTime >= 1)
        {
            m_plateClickAudioSource.PlayOneShot(m_movingEndedClick);
        }

        m_MoveplateCoroutine = null;

        if ( m_curTime / allTime >= 1)
        {
           
            
        }
    }


    private void OnCollisionEnter(Collision collision)
    {

        var colRb = collision.gameObject.GetComponent<Rigidbody>();
        if (!m_currentColliders.Contains(collision.gameObject) && colRb != null)
        {
            m_currentColliders.Add(collision.gameObject);
            var prevMass = m_curMass;
            m_curMass += colRb.mass;

            Moveplate(prevMass / m_maxMass[0], m_curMass / m_maxMass[0], m_stageMoveTime[0]);
            

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        m_currentColliders.Remove(collision.gameObject);
        var prevMass = m_curMass;
        m_curMass -= collision.gameObject.GetComponent<Rigidbody>().mass;
        Moveplate(prevMass / m_maxMass[0], m_curMass / m_maxMass[0], m_stageMoveTime[0]);
    }
}
