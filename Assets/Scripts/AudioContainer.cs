using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioContainer : MonoBehaviour
{
    [SerializeField] protected AudioClip audioRecord;
    public AudioClip AudioRecord 
    { 
        get
        {
            return audioRecord;
        } 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
