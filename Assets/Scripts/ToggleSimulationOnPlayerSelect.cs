using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleSimulationOnPlayerSelect : MonoBehaviour
{
    [SerializeField] private  ParticleSystem m_fire;

    private ParticleSystemCullingMode m_startCullingMode;

    [SerializeField] private GameObject m_player;
    [SerializeField] private string m_PlayerTag = "Player";
    private XRSocketInteractor[] m_playerSockets;
    // Start is called before the first frame update
    void Awake()
    {
        m_startCullingMode = m_fire.main.cullingMode;
        if(m_player==null) m_player = GameObject.FindGameObjectWithTag(m_PlayerTag);
        m_playerSockets = m_player.GetComponent<PlayerSockets>().PlSockets;
    }

    public void ChangeCullingMode(SelectEnterEventArgs args)
    {
        
        if ((args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null) || m_playerSockets.Contains(args.interactorObject))
        {
            var main = m_fire.main;
            main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            
        }
    }
    public void RestoreCullingMode()
    {
        var main = m_fire.main;
        main.cullingMode = m_startCullingMode;
    }
}
