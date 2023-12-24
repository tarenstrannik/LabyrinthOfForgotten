using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationProviderWithClearQueue : TeleportationProvider
{
    private TeleportationProvider m_teleportProvider;


    public void ClearQueue()
    {

        validRequest = false;
    }
}
