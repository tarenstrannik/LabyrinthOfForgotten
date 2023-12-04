using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterSecond : DetectOnTriggerEnter
{
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        m_parentScript.TurnOffWaterCollider();
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
