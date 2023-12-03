using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.StickyNote;

public class LightVariation : MonoBehaviour
{
    [SerializeField] private float m_minIntencity = 5f;
    [SerializeField] private float m_maxIntencity = 12f;

    [SerializeField] private float m_minDistance = 10f;
    [SerializeField] private float m_maxDistance = 20f;

 

    [SerializeField] private float m_minTimeInterval = 0.1f;
    [SerializeField] private float m_maxTimeInterval = 3f;

    [SerializeField] private Color m_minColor;
    [SerializeField] private Color m_maxColor;

    private Light m_light;

    [SerializeField] private bool m_isPlayingOnStart = false;

    private Coroutine m_variationCoroutine;

    [SerializeField] Renderer m_visibilityRenderer;

    private void Awake()
    {
        m_light = GetComponent<Light>();
    }

    private void OnEnable()
    {
        m_light.range= Random.Range(m_minDistance, m_maxDistance);
        m_light.intensity = Random.Range(m_minIntencity, m_maxIntencity);
        m_light.color = Color.Lerp(m_minColor, m_maxColor, Random.value);
        if (m_isPlayingOnStart)
            StartLightVariation();

    }

    public void StartLightVariation()
    {
        m_variationCoroutine = StartCoroutine(VariateLight());
    }
    public void StopLightVariation()
    {
        StopCoroutine(m_variationCoroutine);
    }

    private IEnumerator VariateLight()
    {
        var curInterval = Random.Range(m_minTimeInterval, m_maxTimeInterval);
        var maxInterval = curInterval;

        var startColor = m_light.color;
        var endColor = Color.Lerp(m_minColor, m_maxColor, Random.value);

        var startDistance = m_light.range;
        var endDistance = Random.Range(m_minDistance, m_maxDistance);

        var startIntencity = m_light.intensity;
        var endIntencity = Random.Range(m_minIntencity, m_maxIntencity);


        while (curInterval>=0)
        {
            m_light.range = Mathf.Lerp(startDistance, endDistance, (maxInterval - curInterval) / maxInterval);

            m_light.intensity = Mathf.Lerp(startIntencity, endIntencity, (maxInterval - curInterval) / maxInterval);
            m_light.color = Color.Lerp(startColor, endColor, (maxInterval - curInterval) / maxInterval);

            m_visibilityRenderer.transform.localScale = new Vector3(m_light.range, m_light.range, m_light.range);
            curInterval -= Time.deltaTime;
            yield return null;
        }

        StartCoroutine(VariateLight());
    }
}
