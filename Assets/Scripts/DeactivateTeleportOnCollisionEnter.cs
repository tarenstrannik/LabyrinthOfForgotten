using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateTeleportOnCollisionEnter : MonoBehaviour
{
    [SerializeField] private GameObject m_teleportController;

    private void OnCollisionEnter(Collision collision)
    {
        m_teleportController.SetActive(false);
    }
    private void OnCollisionExit(Collision collision)
    {
        m_teleportController.SetActive(true);
    }
}
