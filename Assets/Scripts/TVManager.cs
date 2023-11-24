using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVManager : MonoBehaviour
{
    [SerializeField] private GameObject screen;
    // Start is called before the first frame update
    private void SwitchTVPlayPause()
    {
        screen.SendMessage("TogglePlayPause", SendMessageOptions.DontRequireReceiver);
    }
}
