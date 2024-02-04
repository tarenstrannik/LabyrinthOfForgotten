using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterFirst : DetectOnTriggerEnter
{
    protected override void OnTriggerEnter(Collider other)
    {
        //base.OnTriggerEnter(other);
        if (other.gameObject.layer == m_playerTeleportHandToFollowLayer)
        {
            m_parentScript.IsFromWater = true;
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        //base.OnTriggerExit(other);
        if (other.gameObject.layer == m_playerTeleportHandToFollowLayer)
        {
            m_parentScript.IsFromWater = false;
        }
    }
}
