using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSockets : MonoBehaviour
{
    [SerializeField] private XRSocketInteractor[] m_playerSockets;

    public List<GameObject> PlayerFiringTorches= new List<GameObject>();
    public List<GameObject> PlayerAllTorches = new List<GameObject>();
    public XRSocketInteractor[] PlSockets
    {
        get
        {
            return m_playerSockets;
        }
    }
}
