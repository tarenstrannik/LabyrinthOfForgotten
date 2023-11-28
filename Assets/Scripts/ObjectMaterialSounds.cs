using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMaterialSounds : MonoBehaviour
{
    [SerializeField] private Materials m_objectMaterial;
    public Materials m_ObjectMaterial
    {
        get
        {
            return m_objectMaterial;
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

[System.Serializable] public enum Materials
{
    none,
    wood,
    stone,
    metall,
    hand
}
