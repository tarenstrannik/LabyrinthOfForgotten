using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorByHandle : MonoBehaviour
{
    [SerializeField] private GameObject m_linkedDoor;

    public void OpenDoor()
    {
        m_linkedDoor.SendMessage("MoveDoorFullFirstStage", SendMessageOptions.DontRequireReceiver);
    }
}
