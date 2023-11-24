using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateHandsCollidersOnAttach : MonoBehaviour
{
    [SerializeField] private List<GameObject> m_PhysicalColliders;
    // Start is called before the first frame update
    [SerializeField] private float m_ActivationDelay = 0.5f;
    [SerializeField] private float m_DeactivationDelay = 0.5f;
    public void EnableColliders()
    {
        foreach(var collider in m_PhysicalColliders)
        {
            collider.SetActive(true);
        }
    }
    public void DisableColliders()
    {
        foreach (var collider in m_PhysicalColliders)
        {
            collider.SetActive(false);
        }
    }
    public void EnableCollidersWithDelay()
    {
        Invoke("EnableColliders", m_ActivationDelay);
    }
    public void DisableCollidersWithDelay()
    {
        Invoke("DisableColliders", m_DeactivationDelay);
    }
}
