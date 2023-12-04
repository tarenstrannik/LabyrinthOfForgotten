using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterDebug : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(this.name);
    }
}
