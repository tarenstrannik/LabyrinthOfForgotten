using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PhotoOperations : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_photoImage;
    // Start is called before the first frame update
    [SerializeField] private float m_timeToRevealPhotoMin = 10f;
    [SerializeField] private float m_timeToRevealPhotoMax = 30f;

    private float m_curPhotoRevealTime = 0;
    private float m_PhotoRevealTime = 0;
    public void SetPhotoImage(Texture2D photoImage)
    {
        m_photoImage.sprite = Sprite.Create(photoImage, new Rect(0, 0, photoImage.width, photoImage.height), new Vector2(0.5f, 0.5f));
    }

    private void OnEnable()
    {
        StartCoroutine(RevealPhoto());
    }
    private IEnumerator RevealPhoto()
    {
        m_curPhotoRevealTime = 0f;
        m_PhotoRevealTime = Random.Range(m_timeToRevealPhotoMin, m_timeToRevealPhotoMax);
        while (m_curPhotoRevealTime< m_PhotoRevealTime)
        {
            Color tmp = m_photoImage.color;
            tmp.a = Mathf.SmoothStep(0f,1f,m_curPhotoRevealTime / m_PhotoRevealTime);
            m_photoImage.color = tmp;
            m_curPhotoRevealTime += Time.deltaTime;
                        yield return null;
        }
        Color tmpFin = m_photoImage.color;
        tmpFin.a = 1;
        m_photoImage.color = tmpFin;
    }
}
