using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RayInteractorHandsFollowDirectController : PhysHandsFollowDirectControllers
{
    [SerializeField] private float m_maxControllerDistanceRayActive = 0.2f;



    [SerializeField] private ToggleObjectRayInteractor m_toggleRay;

    private bool m_isDistanceLower=true;

    protected override void Awake()
    {
        base.Awake();
         
    }

    protected override void FixedUpdate()
    {

        var distance = Vector3.Distance(m_controllerToFollow.position, transform.position);
        var curDistanceLower = distance < m_maxControllerDistanceRayActive;
        if(m_isDistanceLower!= curDistanceLower)
        {
            m_isDistanceLower = curDistanceLower;
            SetRayActive(m_isDistanceLower);
        }

               


            base.FixedUpdate();

    }

    private void SetRayActive(bool val)
    {
        m_toggleRay.ToggleRayInteractor(val);
    }
}
