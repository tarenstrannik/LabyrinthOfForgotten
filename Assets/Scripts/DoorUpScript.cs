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
    [SerializeField] private Transform[]  m_endTransforms;


    [SerializeField] GameObject m_pruty;
    [SerializeField] GameObject m_transparentDoor;

    

    private Coroutine m_MoveDoorCoroutine=null;
    private float m_curTime;

    [SerializeField] private float[] m_stageMoveTime = { 3f, 3f };
    [SerializeField] float m_pauseTime = 3f;

    private float m_startPitch;

    private int m_curStage = 0;
    private void Awake()
    {
        m_doorAudioSource = GetComponent<AudioSource>();
        m_pruty.transform.position = m_startTransforms[0].position;
        m_transparentDoor.transform.position = m_startTransforms[0].position;
        m_startPitch = m_doorAudioSource.pitch;
    }



    public void MoveDoor(float startTimePercent, float endTimePercent, float allTime)
    {
        if (m_curStage == 0)
        {
           
            m_doorAudioSource.pitch = m_startPitch*allTime / m_stageMoveTime[1];
            GameObject[] objectsToMove = { m_pruty };
            if (m_MoveDoorCoroutine != null)
            {
                StopMovingDoorCoroutine(m_MoveDoorCoroutine);
            }
            m_MoveDoorCoroutine = StartCoroutine(IMoveDoor(objectsToMove, startTimePercent, endTimePercent, allTime));
        }
    }
    private void StopMovingDoorCoroutine(Coroutine cor)
    {
        if(cor!=null)
        {
            StopCoroutine(cor);
            m_doorAudioSource.Pause();
        }
    }
    private IEnumerator IMoveDoor(GameObject[] objectsToMove, float startTimePercent, float endTimePercent, float allTime)
    {
        

        Vector3 startPosition = m_startTransforms[m_curStage].position;
        Vector3 endPosition = m_endTransforms[m_curStage].position;


        int directionCoef= endTimePercent> startTimePercent ? 1 : -1;
        
        m_curTime = startTimePercent * allTime;

        m_doorAudioSource.Play();

        while (directionCoef*m_curTime <= directionCoef*endTimePercent * allTime)
        {
            foreach(var obj in objectsToMove)
            {
                
                obj.transform.position = Vector3.Lerp(startPosition, endPosition, m_curTime/ allTime);
            }
            m_curTime += directionCoef*Time.deltaTime;
            
            yield return null;
        }
        m_doorAudioSource.Pause();
        if (m_curTime/ allTime<=0 || m_curTime / allTime >=1)
        {
            m_doorSlamAudioSource.PlayOneShot(m_movingEndedSound);
        }
              
        m_MoveDoorCoroutine = null;

        if(m_curStage==0 && m_curTime / allTime >= 1)
        {
            m_curStage = 1;
            yield return new WaitForSeconds(m_pauseTime);
            GameObject[] obj = { m_pruty, m_transparentDoor };
            m_doorAudioSource.pitch = m_startPitch;
            m_MoveDoorCoroutine =StartCoroutine(IMoveDoor(obj, 0, 1, m_stageMoveTime[m_curStage]));
        }
    }

    public void MoveDoorFullFirstStage()
    {
        m_doorAudioSource.pitch = m_startPitch * m_stageMoveTime[0] / m_stageMoveTime[1];
        MoveDoor(0,1, m_stageMoveTime[m_curStage]);
    }
}
