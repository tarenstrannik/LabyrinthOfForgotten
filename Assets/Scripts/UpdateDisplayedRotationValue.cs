using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateDisplayedRotationValue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_rotationTMP;
    [SerializeField] private GameObject m_pedestal;
    [SerializeField] private Slider m_slider;
    private void FixedUpdate()
    {
        m_rotationTMP.text = ""+(int)m_pedestal.transform.localEulerAngles.y;
        m_slider.value= m_pedestal.transform.localEulerAngles.y;
    }
}
