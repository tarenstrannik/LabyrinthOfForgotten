using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
public class DoorUpScript : MonoBehaviour, IHaveMinMax, IMoveLinear
{
    [SerializeField] private Transform[] m_startTransforms;
    [SerializeField] private Transform[] m_endTransforms;


    [SerializeField] GameObject m_pruty;
    [SerializeField] GameObject m_transparentDoor;

    private Coroutine m_curCoroutine = null;

    [SerializeField] private float[] m_stageMovingUpTime = { 3f, 6f };
    [SerializeField] private float[] m_stageMovingDownTime = { 3f, 1f };
    [SerializeField] float m_pauseBetweenStagesUpTime = 3f;
    [SerializeField] float m_pauseBetweenStagesDownTime = 0f;

    private int m_curStage = 0;
    private float m_curPercent = 0;
    private float m_cachedTargetPercent = 0;

    // Events
    [Header("Events options")]

    [SerializeField] private UnityEvent m_fullyOpenedReachedEvent;
    public UnityEvent OnMaxReachedEvent
    {
        get
        {
            return m_fullyOpenedReachedEvent;
        }

    }
    [SerializeField] private UnityEvent m_fullyOpenedLeftEvent;
    public UnityEvent OnMaxLeftEvent
    {
        get
        {
            return m_fullyOpenedLeftEvent;
        }

    }

    [SerializeField] private UnityEvent<float> m_positionChangeBeginEvent;
    public UnityEvent<float> OnPositionChangeBegin
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

    [SerializeField] private UnityEvent m_startPositionLeftEvent;
    public UnityEvent OnMinLeftEvent 
    {
        get
        {
            return m_startPositionLeftEvent;
        }
    }


    [SerializeField] private UnityEvent m_intermediatePositionReachedFromMinEvent;
    public UnityEvent OnIntermediateFromMinReachedEvent
    {
        get
        {
            return m_intermediatePositionReachedFromMinEvent;
        }
    }
    

    [SerializeField] private UnityEvent m_intermediateToMinPositionLeftEvent;
    public UnityEvent OnIntermediateToMinLeftEvent
    {
        get
        {
            return m_intermediateToMinPositionLeftEvent;
        }
    }


    [SerializeField] private UnityEvent m_intermediatePositionReachedFromMaxEvent;
    public UnityEvent OnIntermediateFromMaxReachedEvent
    {
        get
        {
            return m_intermediatePositionReachedFromMaxEvent;
        }
    }

    [SerializeField] private UnityEvent m_intermediateToMaxPositionLeftEvent;
    public UnityEvent OnIntermediateToMaxLeftEvent
    {
        get
        {
            return m_intermediateToMaxPositionLeftEvent;
        }
    }

    [SerializeField] private UnityEvent[] m_goingUp;
    [SerializeField] private UnityEvent[] m_goingDown;

    //----------------

    private void Awake()
    {
        m_pruty.transform.position = m_startTransforms[0].position;
        m_transparentDoor.transform.position = m_startTransforms[0].position;        
    }

    private void OnEnable()
    {
        OnIntermediateFromMinReachedEvent.AddListener(SwitchStageWithDelay);
        OnIntermediateFromMaxReachedEvent.AddListener(SwitchStageWithDelay);
    }
    private void OnDisable()
    {
        OnIntermediateFromMinReachedEvent.RemoveListener(SwitchStageWithDelay);
        OnIntermediateFromMaxReachedEvent.RemoveListener(SwitchStageWithDelay);
    }
    public void MoveDoor(float endTimePercent)
    {
        if (m_curCoroutine != null) StopCoroutine(m_curCoroutine);
        float fullStageMovingTime = 0;
        if (endTimePercent >= m_curPercent)
        {
            m_goingUp[m_curStage].Invoke();
            fullStageMovingTime = m_stageMovingUpTime[m_curStage];
        }
        else
        {
            m_goingDown[m_curStage].Invoke();
            fullStageMovingTime = m_stageMovingDownTime[m_curStage];
        }
        GameObject[] objectsToMove;
        float targetPercent = 0f;
        if (m_curStage==0)
        {
            GameObject[] obj = { m_pruty };
            objectsToMove = obj;

            if(m_curPercent==0)
            {
                OnMinLeftEvent.Invoke();
            }
            else if (m_curPercent == 1)
            {
                OnIntermediateToMinLeftEvent.Invoke();
            }

            targetPercent = endTimePercent;
        }
        else
        {
            GameObject[] obj = { m_pruty, m_transparentDoor };
            objectsToMove = obj;
            if (m_curPercent == 0)
            {
                OnIntermediateToMaxLeftEvent.Invoke();
            }
            else if (m_curPercent == 1)
            {
                OnMaxLeftEvent.Invoke();
            }
            if(endTimePercent==1)
            {
                targetPercent = endTimePercent;
            }
            else
            {
                m_cachedTargetPercent = endTimePercent;
                targetPercent = 0;
            }
        }

        m_curCoroutine = StartCoroutine(IMoveDoor(objectsToMove, targetPercent, fullStageMovingTime));

        OnPositionChangeBegin.Invoke(endTimePercent);

    }
    private IEnumerator IMoveDoor(GameObject[] objectsToMove, float endTimePercent, float allTime)
    {
        Vector3 startPosition = m_startTransforms[m_curStage].position;
        Vector3 endPosition = m_endTransforms[m_curStage].position;


        int directionCoef = endTimePercent > m_curPercent ? 1 : -1;
        
        while (directionCoef * m_curPercent <= directionCoef * endTimePercent)
        {
            foreach (var obj in objectsToMove)
            {

                obj.transform.position = Vector3.Lerp(startPosition, endPosition, m_curPercent);
            }
            m_curPercent += directionCoef * Time.deltaTime / allTime;
            yield return null;
        }

        m_curPercent = endTimePercent;
        m_curPercent = m_curPercent > 1 ? 1 : m_curPercent;
        m_curPercent = m_curPercent < 0 ? 0 : m_curPercent;

        OnPositionChangeEnd.Invoke();
        if (m_curStage == 0)
        {
            if (m_curPercent <= 0)
            {
                OnMinReachedEvent.Invoke();
            }
            else if (m_curPercent >= 1)
            {
                OnIntermediateFromMinReachedEvent.Invoke();
            }
        }
        else if (m_curStage == 1)
        {
            if (m_curPercent <= 0)
            {
                OnIntermediateFromMaxReachedEvent.Invoke();
            }
            else if (m_curPercent >= 1)
            {
                OnMaxReachedEvent.Invoke();
            }
        }
    }

    private void SwitchStageWithDelay()
    {
        m_curCoroutine=StartCoroutine(ISwitchStageWithDelay());
    }
    private IEnumerator ISwitchStageWithDelay()
    {
        if (m_curStage == 0)
        {
            
            yield return new WaitForSeconds(m_pauseBetweenStagesUpTime);
            m_curStage = 1;
            m_curPercent = 0;
            MoveDoor(1);
        }
        else
        {
            yield return new WaitForSeconds(m_pauseBetweenStagesDownTime);
            m_curStage = 0;
            m_curPercent = 1;
            MoveDoor(m_cachedTargetPercent);
        }
        m_transparentDoor.transform.position = m_startTransforms[m_curStage].position;
    }
    


     
}