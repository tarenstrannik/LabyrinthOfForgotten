using System;
using System.Collections;
using System.Collections.Generic;

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

    private bool m_isFalling=false;
    public bool m_IsFalling
    {
        get
        {
            return m_isFalling;
        }
    }

    private Coroutine m_MoveDoorCoroutine = null;

    private float m_curTime=0;
    private float m_curPercent=0;


    [SerializeField] private float[] m_stageMoveTime = { 3f, 3f };
    [SerializeField] float m_pauseTime = 3f;
    [SerializeField] float m_MoveDownTimeSecondStage = 1f;


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



    public void MoveDoor(float endTimePercent, float allTime)
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
            m_MoveDoorCoroutine = StartCoroutine(IMoveDoor(objectsToMove, endTimePercent, allTime));

    }
    public void MoveDoorSecondStageDown()
    {
        if (m_curStage == 1)
        {
            m_transparentDoor.transform.position= m_startTransforms[0].position;
            m_doorAudioSource.pitch = m_startPitch * m_stageMoveTime[1] / m_MoveDownTimeSecondStage;

            GameObject[] obj = { m_pruty }; //GameObject[] obj = { m_pruty, m_transparentDoor };
            GameObject[] objectsToMove = obj;
            

            if (m_MoveDoorCoroutine != null)
            {
                StopMovingDoorCoroutine();
            }
            m_MoveDoorCoroutine = StartCoroutine(IMoveDoor(objectsToMove, 0, m_MoveDownTimeSecondStage));
        }
    }
    public void StopMovingDoorCoroutine()
    {
        if (m_MoveDoorCoroutine != null)
        {
            StopCoroutine(m_MoveDoorCoroutine);
            m_doorAudioSource.Pause();
            m_MoveDoorCoroutine = null;
            m_isFalling = false;
        }
    }
    private IEnumerator IMoveDoor(GameObject[] objectsToMove, float endTimePercent, float allTime)
    {
        if(endTimePercent < m_curPercent && m_curStage==1)
        {
            m_isFalling = true;
        }

        Vector3 startPosition = m_startTransforms[m_curStage].position;
        Vector3 endPosition = m_endTransforms[m_curStage].position;


        int directionCoef = endTimePercent > m_curPercent ? 1 : -1;

        m_curTime = m_curPercent * allTime;

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
        m_curPercent = m_curPercent > 1 ? 1 : m_curPercent;
        m_curPercent = m_curPercent < 0 ? 0 : m_curPercent;
        m_doorAudioSource.Pause();
        if (m_curPercent <= 0 || m_curPercent >= 1)
        {
            m_doorSlamAudioSource.PlayOneShot(m_movingEndedSound);
        }     
        
        if (m_curTime / allTime >= 1 && m_curStage==0)
        {
            var curPause = m_pauseTime;
            while(m_pauseTime>=0)
            {
                m_pauseTime -= Time.deltaTime;
                yield return null;
            }
            //yield return new WaitForSeconds(m_pauseTime);
            m_curStage = 1;
            m_curPercent = 0;
            m_MoveDoorCoroutine = null;
            MoveDoorSecondStageUp();
        }
        else if (m_curTime / allTime<=0 && m_curStage ==1)
        {
            m_curStage = 0;
            //m_transparentDoor.transform.position= m_startTransforms[0].position;
            m_curPercent = 1;
            m_isFalling = false;
            m_MoveDoorCoroutine = null;
        }
        
    }
    /*--------------------for handle--------------*/
    public void MoveDoorFullFirstStage()
    {
        m_doorAudioSource.pitch = m_startPitch * m_stageMoveTime[0] / m_stageMoveTime[1];
        MoveDoor(1, m_stageMoveTime[m_curStage]);

        
    }
    public void MoveDoorSecondStageUp()
    {
        
        GameObject[] obj = { m_pruty, m_transparentDoor };
        m_doorAudioSource.pitch = m_startPitch;

        m_MoveDoorCoroutine = StartCoroutine(IMoveDoor(obj, 1, m_stageMoveTime[m_curStage] * (1 - m_curPercent)));
    }

    

     
}