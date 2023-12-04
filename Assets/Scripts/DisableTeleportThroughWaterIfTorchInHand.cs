using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableTeleportThroughWaterIfTorchInHand : MonoBehaviour
{
    [SerializeField] private GameObject m_WaterCollider;
    [SerializeField] private string m_torchTag;

    [SerializeField] private GameObject m_player;
    [SerializeField] private string m_PlayerTag = "Player";
    private PlayerSockets m_playerSockets;
    void Awake()
    {
       
        if (m_player == null) m_player = GameObject.FindGameObjectWithTag(m_PlayerTag);
        m_playerSockets= m_player.GetComponent<PlayerSockets>();
    }
    public void EnableColliderOnTorchSelect(SelectEnterEventArgs args)
    {
       if(args.interactableObject.transform.CompareTag(m_torchTag))
        {

            m_playerSockets.m_playerAllTorches.Add(args.interactableObject.transform.gameObject);
            if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().Fire.isPlaying)
            {
                m_playerSockets.m_playerFiringTorches.Add(args.interactableObject.transform.gameObject);
            }
        }
        m_WaterCollider.SetActive(m_playerSockets.m_playerFiringTorches.Count > 0);
    }
    private void AddPlayerFiringTorchAndEnableWaterCollider(GameObject obj)
    {

        if (m_playerSockets.m_playerAllTorches.Contains(obj))
        {
            m_playerSockets.m_playerFiringTorches.Add(obj);
        }
        m_WaterCollider.SetActive(m_playerSockets.m_playerFiringTorches.Count > 0);

    }
    private void RemovePlayerFiringTorchAndDisableWaterCollider(GameObject obj)
    {

        if (m_playerSockets.m_playerAllTorches.Contains(obj))
        {
            m_playerSockets.m_playerFiringTorches.Remove(obj);
        }
        m_WaterCollider.SetActive(m_playerSockets.m_playerFiringTorches.Count > 0);

    }
    public void DisableColliderOnTorchDeSelect(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag(m_torchTag))            
        {
            m_playerSockets.m_playerAllTorches.Remove(args.interactableObject.transform.gameObject);
            if (args.interactableObject.transform.gameObject.GetComponentInChildren<IgniteFire>().Fire.isPlaying)
            {
                m_playerSockets.m_playerFiringTorches.Remove(args.interactableObject.transform.gameObject);
            }
        }

        m_WaterCollider.SetActive(m_playerSockets.m_playerFiringTorches.Count > 0);

    }


}
