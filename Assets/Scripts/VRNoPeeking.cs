using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class VRNoPeeking : MonoBehaviour
{
    [SerializeField] LayerMask m_collisionLayer;
    [SerializeField] private float m_fadeSpeed;
    [SerializeField] private float m_sphereCheckSize=0.15f;

    private Material m_cameraFadeMat;
    private bool m_isCameraFadeOut;
    private void Awake() => m_cameraFadeMat = GetComponent<Renderer>().material;


    private XROrigin m_XROrigin;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Physics.CheckSphere(transform.position, m_sphereCheckSize, m_collisionLayer, QueryTriggerInteraction.Ignore))
        {
            CameraFade(1f);
            m_isCameraFadeOut = true;
        }
        else
        {
            if (!m_isCameraFadeOut)
                return;
            CameraFade(0f);
        }
    }

    public void CameraFade(float targetAlpha)
    {
        var fadeValue = Mathf.MoveTowards(m_cameraFadeMat.GetFloat("_AlphaValue"), targetAlpha, Time.deltaTime * m_fadeSpeed);
        m_cameraFadeMat.SetFloat("_AlphaValue", fadeValue);

        if (fadeValue <= 0.01f)
            m_isCameraFadeOut = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.75f);
        Gizmos.DrawSphere(transform.position, m_sphereCheckSize);
    }
}
