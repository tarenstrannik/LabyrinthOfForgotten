using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SaveLinkOnControllerSelectedObject : MonoBehaviour
{
    private GameObject m_controller = null;
    
    public GameObject Controller
    {
        get
        {
            return m_controller;
        }

    }
    [SerializeField] private string m_PlayerTag = "Player";
    [SerializeField] private GameObject m_player;

    private XRSocketInteractor[] m_playerSockets;
    private void Awake()
    {
        if (m_player == null) m_player = GameObject.FindGameObjectWithTag(m_PlayerTag);
        m_playerSockets = m_player.GetComponent<PlayerSockets>().PlSockets;
    }
    public void SaveLinkToController(SelectEnterEventArgs args)
    {
        if ((args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)|| m_playerSockets.Contains(args.interactorObject))
        {
            m_controller = args.interactorObject.transform.gameObject;
            
        }
    }
    public void ClearLinkToController()
    {
        m_controller = null; 
    }

}
