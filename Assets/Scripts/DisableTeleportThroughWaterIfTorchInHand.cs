using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableTeleportThroughWaterIfTorchInHand : MonoBehaviour
{
    [SerializeField] private GameObject[] m_waterColliders;
    [SerializeField] private string m_torchTag;

    [SerializeField] private GameObject m_player;
    [SerializeField] private string m_playerTag = "Player";

    private PlayerSockets m_playerSockets;
    void Awake()
    {
       
        if(m_player == null) m_player = GameObject.FindGameObjectWithTag(m_playerTag);
        m_playerSockets= m_player.GetComponent<PlayerSockets>();

    }
    public void EnableColliderOnTorchSelect(SelectEnterEventArgs args)
    {
       if(args.interactableObject.transform.CompareTag(m_torchTag))
        {

            m_playerSockets.PlayerAllTorches.Add(args.interactableObject.transform.gameObject);
            if (args.interactableObject.transform.gameObject.GetComponentInChildren<ICanBlockTeleportIfActiveAndInHand>().IsActive)
            {
                m_playerSockets.PlayerFiringTorches.Add(args.interactableObject.transform.gameObject);
            }
        }
        ToggleNoTeleportCollider(m_playerSockets.PlayerFiringTorches.Count > 0);
    }
    private void AddPlayerFiringTorchAndEnableWaterCollider(GameObject obj)
    {

        if (m_playerSockets.PlayerAllTorches.Contains(obj))
        {
            m_playerSockets.PlayerFiringTorches.Add(obj);
        }
        ToggleNoTeleportCollider(m_playerSockets.PlayerFiringTorches.Count > 0);

    }
    private void RemovePlayerFiringTorchAndDisableWaterCollider(GameObject obj)
    {

        if (m_playerSockets.PlayerAllTorches.Contains(obj))
        {
            m_playerSockets.PlayerFiringTorches.Remove(obj);
        }
        ToggleNoTeleportCollider(m_playerSockets.PlayerFiringTorches.Count > 0);

    }
    public void DisableColliderOnTorchDeSelect(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag(m_torchTag))            
        {
            m_playerSockets.PlayerAllTorches.Remove(args.interactableObject.transform.gameObject);
            if (args.interactableObject.transform.gameObject.GetComponentInChildren<ICanBlockTeleportIfActiveAndInHand>().IsActive)
            {
                m_playerSockets.PlayerFiringTorches.Remove(args.interactableObject.transform.gameObject);
            }
        }

        ToggleNoTeleportCollider(m_playerSockets.PlayerFiringTorches.Count > 0);

    }

    private void ToggleNoTeleportCollider(bool activity)
    {
        foreach(GameObject obj in m_waterColliders)
        {
            obj.SetActive(activity);
        }
    }
}
