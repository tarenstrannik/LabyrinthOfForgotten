using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlNoTeleportCollider : MonoBehaviour
{
    [SerializeField] private GameObject[] m_teleportBlockingColliders;
    [SerializeField] private GameObject m_player;
    [SerializeField] private string m_playerTag = "Player";
    private ISwitchTeleportBlockNeed m_switchTeleportBlockNeed;
    private void Awake()
    {
        if (m_player == null) m_player = GameObject.FindGameObjectWithTag(m_playerTag);
        m_switchTeleportBlockNeed = m_player.GetComponentInChildren<ISwitchTeleportBlockNeed>();
        
    }
    private void OnEnable()
    {
        m_switchTeleportBlockNeed.OnTeleportBlock.AddListener(ActivateNoTeleportCollider);
        m_switchTeleportBlockNeed.OnTeleportUnblock.AddListener(DeactivateNoTeleportCollider);
    }
    private void OnDisable()
    {
        m_switchTeleportBlockNeed.OnTeleportBlock.RemoveListener(ActivateNoTeleportCollider);
        m_switchTeleportBlockNeed.OnTeleportUnblock.RemoveListener(DeactivateNoTeleportCollider);
    }
    private void ActivateNoTeleportCollider()
    {
        foreach (GameObject obj in m_teleportBlockingColliders)
        {
            obj.SetActive(true);
        }
    }
    private void DeactivateNoTeleportCollider()
    {
        foreach (GameObject obj in m_teleportBlockingColliders)
        {
            obj.SetActive(false);
        }
    }
}
