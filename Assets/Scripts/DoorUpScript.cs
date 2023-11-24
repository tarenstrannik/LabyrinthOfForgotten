using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DoorUpScript : MonoBehaviour
{
    [SerializeField] private AudioSource m_doorAudioSource;
    [SerializeField] private AudioSource m_doorSlamAudioSource;
    [SerializeField] private AudioClip m_movingEndedSound;


    [SerializeField] private Transform[] m_startTransforms;
    [SerializeField] private Transform[] m_endTransforms;


    [SerializeField] GameObject m_pruty;
    [SerializeField] GameObject m_transparentDoor;



    private Coroutine m_MoveDoorCoroutine = null;
    private Coroutine m_MoveDoorSecondStageCoroutine = null;
    private float m_curTime;
    private float m_curPercent;

    [SerializeField] private float[] m_stageMoveTime = { 3f, 3f };
    [SerializeField] float m_pauseTime = 3f;
    [SerializeField] float m_MoveDownTimeSecondStage = 1f;
    public float MoveDownTimeSecondStage { get; }

    private float m_startPitch;

    private int m_curStage = 0;
    private int m_maxStage = 1;
    private void Awake()
    {
        m_doorAudioSource = GetComponent<AudioSource>();
        m_pruty.transform.position = m_startTransforms[0].position;
        m_transparentDoor.transform.position = m_startTransforms[0].position;
        m_startPitch = m_doorAudioSource.pitch;
        m_maxStage = m_stageMoveTime.Length - 1;
    }



    public void MoveDoor(float startTimePercent, float endTimePercent, float allTime)
    {


            m_doorAudioSource.pitch = m_startPitch * allTime / m_stageMoveTime[1];
        GameObject[] objectsToMove;
            if (m_curStage==0)
        {
            GameObject[] obj = { m_pruty };
            objectsToMove = obj;
        }
        else
        {
            GameObject[] obj = { m_pruty, m_transparentDoor };
            objectsToMove = obj;
        }
        
            if (m_MoveDoorCoroutine != null)
            {
                StopMovingDoorCoroutine();
            }
            m_MoveDoorCoroutine = StartCoroutine(IMoveDoor(objectsToMove, startTimePercent, endTimePercent, allTime));

    }
    public void MoveDoorSecondStageDown()
    {
        m_doorAudioSource.pitch = m_startPitch * m_MoveDownTimeSecondStage / m_stageMoveTime[1];

            GameObject[] obj = { m_pruty, m_transparentDoor };
        GameObject[] objectsToMove = obj;


        if (m_MoveDoorCoroutine != null)
        {
            StopMovingDoorCoroutine();
        }
        m_MoveDoorCoroutine = StartCoroutine(IMoveDoor(objectsToMove, m_curPercent, 0, m_MoveDownTimeSecondStage*(1- m_curPercent)));

    }
    public void StopMovingDoorCoroutine()
    {
        if (m_MoveDoorCoroutine != null)
        {
            StopCoroutine(m_MoveDoorCoroutine);
            m_doorAudioSource.Pause();
            m_MoveDoorCoroutine = null;
        }
    }
    private IEnumerator IMoveDoor(GameObject[] objectsToMove, float startTimePercent, float endTimePercent, float allTime)
    {


        Vector3 startPosition = m_startTransforms[m_curStage].position;
        Vector3 endPosition = m_endTransforms[m_curStage].position;


        int directionCoef = endTimePercent > startTimePercent ? 1 : -1;

        m_curTime = startTimePercent * allTime;
        m_curPercent = startTimePercent;
        m_doorAudioSource.Play();

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
        m_doorAudioSource.Pause();
        if (m_curTime / allTime <= 0 || m_curTime / allTime >= 1)
        {
            m_doorSlamAudioSource.PlayOneShot(m_movingEndedSound);
        }

        m_MoveDoorCoroutine = null;

        if (m_curTime / allTime >= 1 && m_curStage==0)
        {
            m_curStage = 1;
            m_curPercent = 0;
        }
        else if (m_curTime / allTime<=0 && m_curStage ==1)
        {
            m_curStage = 0;
            m_curPercent = 1;
        }
    }
    /*--------------------for handle--------------*/
    public void MoveDoorFullFirstStage()
    {
        m_doorAudioSource.pitch = m_startPitch * m_stageMoveTime[0] / m_stageMoveTime[1];
        MoveDoor(0, 1, m_stageMoveTime[m_curStage]);

        MoveDoorSecondStageUp();
    }
    public void MoveDoorSecondStageUp()
    {
        if(m_MoveDoorSecondStageCoroutine!=null)
        {
            StopSecondStage();
        }
        m_MoveDoorSecondStageCoroutine=StartCoroutine(WaitAndInitSecondStage());
    }

    
    public void StopSecondStage()
    {
        StopCoroutine(m_MoveDoorSecondStageCoroutine);
        m_MoveDoorSecondStageCoroutine = null;
    }
    private IEnumerator WaitAndInitSecondStage()
    {
        while (m_curStage == 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(m_pauseTime);
        GameObject[] obj = { m_pruty, m_transparentDoor };
        m_doorAudioSource.pitch = m_startPitch;
        
        m_MoveDoorCoroutine = StartCoroutine(IMoveDoor(obj, m_curPercent, 1, m_stageMoveTime[m_curStage]*(1- m_curPercent)));
    }

     
}