using System;
using UnityEngine;
using UnityEngine.Events;
using static EulerAngleFunctions;

/// <summary>
/// When an object is EulerAngleed, run some functionality
/// Used with a grabable object
/// </summary>
public class OnEulerAngle : MonoBehaviour
{
    [Tooltip("EulerAngle range, 0 - 180 degrees")]
    [Range(0, 360)] public float m_MinThreshold = 0.0f;
    [Range(0, 360)] public float m_MaxThreshold = 0.0f;
    [Serializable] public class EulerAngleEvent : UnityEvent<MonoBehaviour> { }

    // Threshold has been broken
    public EulerAngleEvent OnBegin = new EulerAngleEvent();

    public EulerAngleEvent OnContinue = new EulerAngleEvent();

    // Threshold is no longer broken
    public EulerAngleEvent OnEnd = new EulerAngleEvent();

    private bool withinThreshold = false;

    private void Update()
    {
        CheckEulerAngleOrientation();
    }

    private void CheckEulerAngleOrientation()
    {
        float curEulerX = GetXDegrees(transform);
        bool thresholdCheck = curEulerX >= m_MinThreshold && curEulerX <= m_MaxThreshold;

        if (withinThreshold != thresholdCheck)
        {
            withinThreshold = thresholdCheck;

            if (withinThreshold)
            {
                OnBegin.Invoke(this);
            }
            else
            {
                OnEnd.Invoke(this);
            }
        }
        else if(withinThreshold)
        {
            OnContinue.Invoke(this);
        }
    }
}
