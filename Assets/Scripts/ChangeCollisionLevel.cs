using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class ChangeCollisionLevel : MonoBehaviour
{
    private Coroutine m_changingLevelCoroutine = null;
    public void SetCollisionLevel(int newLevel, float delay)
    {
        
        if (m_changingLevelCoroutine != null) StopCoroutine(m_changingLevelCoroutine);
        m_changingLevelCoroutine = StartCoroutine(SetCollisionLevelWithDelay(newLevel, delay));
    }
    private IEnumerator SetCollisionLevelWithDelay(int newLevel, float delay)
    {
        var curDelay = delay;
        while(curDelay > 0)
        {
            curDelay -= Time.deltaTime;
            yield return null;
        }

        transform.gameObject.SetLayerRecursively(newLevel);
    }
}
