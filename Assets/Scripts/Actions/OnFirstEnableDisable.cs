using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFirstEnableDisable : MonoBehaviour
{

    private void Awake()
    {
        gameObject.SetActive(false);
    }
}
