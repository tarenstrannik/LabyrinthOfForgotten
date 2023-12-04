using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportHandsFollowDirectController : PhysHandsFollowDirectControllers
{
    [SerializeField] private float m_maxControllerDistanceTeleportActive = 0.1f;

    protected XRRayInteractor m_teleportRayInteractor;

    protected override void Awake()
    {
        base.Awake();
        m_teleportRayInteractor=GetComponent<XRRayInteractor>();    
    }

    protected override void FixedUpdate()
    {

            var distance = Vector3.Distance(m_controllerToFollow.position, transform.position);


                SetTeleportingActive(!(distance >= m_maxControllerDistanceTeleportActive));


            base.FixedUpdate();

    }

    private void SetTeleportingActive(bool val)
    {
        m_teleportRayInteractor.enabled = val;
    }
}
