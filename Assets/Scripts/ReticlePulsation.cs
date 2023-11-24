using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticlePulsation : MonoBehaviour
{
    [SerializeField] private float pulsationCicleTime=3f;
    private float curPulsationTime;

    [SerializeField] private float maxSizeCoef = 1.2f;

    [SerializeField] private float minSizeCoef = 0.8f;

    private void OnEnable()
    {
        curPulsationTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float curCoef = 0f;
        if (curPulsationTime<= pulsationCicleTime/4)
        {
            curCoef = 1 * (1 - 4 * (curPulsationTime) / pulsationCicleTime) + maxSizeCoef * 4 * (curPulsationTime) / pulsationCicleTime;
            
        }
        else if(curPulsationTime> pulsationCicleTime / 4 && curPulsationTime< pulsationCicleTime * 3/4)
        {
            curCoef = maxSizeCoef*(1-2*(curPulsationTime- pulsationCicleTime / 4)/ pulsationCicleTime) + minSizeCoef* 2 * (curPulsationTime - pulsationCicleTime / 4) / pulsationCicleTime;
        }
        else
        {
            curCoef = minSizeCoef * (1 - 4*(curPulsationTime - 3*pulsationCicleTime / 4) / pulsationCicleTime) + 1 * 4 * (curPulsationTime - 3*pulsationCicleTime / 4) / pulsationCicleTime;

        }
        transform.localScale = new Vector3(curCoef, 1, curCoef);
        curPulsationTime += Time.deltaTime;
        if (curPulsationTime > pulsationCicleTime) curPulsationTime = 0f;
    }
}
