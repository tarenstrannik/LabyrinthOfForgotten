using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSockets : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor[] m_playerSockets;

    public List<GameObject> m_playerFiringTorches= new List<GameObject>();
    public List<GameObject> m_playerAllTorches = new List<GameObject>();
    public XRSocketInteractor[] PlSockets
    {
        get
        {
            return m_playerSockets;
        }
    }
}
