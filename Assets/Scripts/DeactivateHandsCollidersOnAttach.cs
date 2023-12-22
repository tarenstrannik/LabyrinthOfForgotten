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
        CancelInvoke();
        foreach (var collider in m_PhysicalColliders)
        {
            collider.SetActive(true);
        }
    }
    public void DisableColliders()
    {
        CancelInvoke();
        foreach (var collider in m_PhysicalColliders)
        {
            collider.SetActive(false);
        }
    }
    public void EnableCollidersWithDelay()
    {
        CancelInvoke();
        Invoke("EnableColliders", m_ActivationDelay);
    }
    public void DisableCollidersWithDelay()
    {
        CancelInvoke();
        Invoke("DisableColliders", m_DeactivationDelay);
    }
}
