using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfVisible : MonoBehaviour
{

    [SerializeField] private FireController m_fireController;
    private Light m_fireLight;
    [SerializeField] private int m_invisibleRendererLayer = 22;
    private void Awake()
    {
        m_fireLight = m_fireController.FireLight;
        var visDistance = m_fireController.FireLight.range;

        transform.localScale = new Vector3(visDistance, visDistance, visDistance);
        
    }

    private void Start()
    {
        
        if(!GetComponent<Renderer>().isVisible)
        {

                m_fireLight.enabled = false;
        }
        
    }
    
    private void OnBecameVisible()
    {

        if (m_fireController.IsActive) m_fireLight.enabled = true;

    }

    private void OnBecameInvisible()
    {

        m_fireLight.enabled = false;

    }
    private void Update()
    {
        gameObject.layer = m_invisibleRendererLayer;
    }
}
