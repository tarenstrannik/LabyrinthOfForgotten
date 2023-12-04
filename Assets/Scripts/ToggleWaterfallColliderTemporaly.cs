using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWaterfallColliderTemporaly : MonoBehaviour
{
    [SerializeField] GameObject m_waterCollider;
    private bool m_isFromWater = false;
    public bool IsFromWater
    {
        get 
        { 
            return m_isFromWater;
        }
        set
        {
            m_isFromWater = value;
        }
    }
    private bool m_isHandHere = false;
    public bool IsHandHere
    {
        get
        {
            return m_isHandHere;
        }
        set
        {
            m_isHandHere = value;
        }
    }

    private Coroutine m_turnOffWaterColliderCoroutine;
    public void TurnOffWaterCollider()
    {
        if (m_waterCollider.activeSelf)
        {
            if (m_isFromWater)
            {
                if (m_turnOffWaterColliderCoroutine != null)
                {

                    StopCoroutine(m_turnOffWaterColliderCoroutine);
                }
                m_turnOffWaterColliderCoroutine = StartCoroutine(ITurnOffWaterCollider());
            }
        }
    }
    private IEnumerator ITurnOffWaterCollider()
    {
        m_waterCollider.SetActive(false);
  
        while (!m_isHandHere )
        {

            yield return null;
        }


        m_waterCollider.SetActive(true);
        m_turnOffWaterColliderCoroutine = null;
    }
}
