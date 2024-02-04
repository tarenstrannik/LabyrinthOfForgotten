using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMaterialSounds : MonoBehaviour
{
    [SerializeField] private Materials m_objectMaterial;
    public Materials ObjectMaterial
    {
        get
        {
            return m_objectMaterial;
        }
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
