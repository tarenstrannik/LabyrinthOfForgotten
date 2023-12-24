using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableCameraIfPlayerNotLooking : MonoBehaviour
{
    [SerializeField] private GameObject m_mirrorCamera;
    private void Start()
    {

        if (!GetComponent<Renderer>().isVisible)
        {

            m_mirrorCamera.SetActive(false);
        }

    }
    private void OnBecameVisible()
    {

        m_mirrorCamera.SetActive(true);

    }

    private void OnBecameInvisible()
    {

        m_mirrorCamera.SetActive(false);

    }
}
