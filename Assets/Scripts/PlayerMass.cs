using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMass : MonoBehaviour
{
    [SerializeField] private float m_playerMass = 80f;
    public float Mass
    {
        get
        {
            return m_playerMass;
        }
    }
}
