using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendCommandToTV : MonoBehaviour
{
    [SerializeField] private float interactiionDistance = 5f;
    [SerializeField] private GameObject raySource;

    public void SendControlRayPlayPause()
    {
        GameObject tv = SendRay();
        tv.SendMessageUpwards("SwitchTVPlayPause", SendMessageOptions.DontRequireReceiver);
    }
    private GameObject? SendRay()
    {
        RaycastHit hit;
        if (Physics.Raycast(raySource.transform.position, raySource.transform.TransformDirection(Vector3.forward), out hit, interactiionDistance))
        {

            if (hit.collider.gameObject.tag == "TV")
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }
}


