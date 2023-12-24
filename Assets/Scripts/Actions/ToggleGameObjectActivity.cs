using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleGameObjectActivity : MonoBehaviour
{
    public void ToggleActivity()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
