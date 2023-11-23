using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CameraScript : MonoBehaviour
{

    [SerializeField] private Light m_flashLight;
    [SerializeField] private float m_flashTime = 0.1f;

    private bool m_isPrinting = false;

    private AudioSource m_audioSource = null;

    [SerializeField] private Transform m_shotStartTransform;
    [SerializeField] private Transform m_shotEndTransform;
    [SerializeField] private GameObject m_shotPrefab;

    private GameObject m_currentShot=null;

    private float m_shottingTime;
    private float m_curShottingTime;


    [SerializeField] private Camera shotCamera;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_shottingTime = m_audioSource.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeShot()
    {
        if (!m_isPrinting)
        {
            m_isPrinting = true;
            m_currentShot = Instantiate(m_shotPrefab, m_shotStartTransform.position, m_shotStartTransform.rotation);
            m_audioSource.Play();
            m_flashLight.enabled = true;
            Invoke("TakeShot", m_flashTime / 2);
            Invoke("DisableFlash", m_flashTime);
            StartCoroutine(PrintingShot());
        }

    }
    private void DisableFlash()
    {
        m_flashLight.enabled = false;
    }
    private IEnumerator PrintingShot()
    {
        m_curShottingTime = 0f;
        while (m_curShottingTime< m_shottingTime)
        {
            m_currentShot.transform.position = Vector3.Lerp(m_shotStartTransform.position, m_shotEndTransform.position, m_curShottingTime / m_shottingTime);
            m_currentShot.transform.rotation = m_shotStartTransform.rotation;
            m_curShottingTime += Time.deltaTime;
            yield return null;
        }
        m_currentShot.transform.position = m_shotEndTransform.position;
        m_currentShot.GetComponent<Rigidbody>().isKinematic = false;
        m_currentShot.GetComponent<BoxCollider>().enabled = true;
        m_currentShot.GetComponent<XRGrabInteractable>().enabled = true;
        m_isPrinting = false;
    }

    private void TakeShot()
    {
        m_currentShot.GetComponent<PhotoOperations>().SetPhotoImage(RTImage(shotCamera));
    }


    Texture2D RTImage(Camera camera)
    {
        // The Render Texture in RenderTexture.active is the one
        // that will be read by ReadPixels.
        var currentRT = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        // Render the camera's view.
        camera.Render();

        // Make a new texture and read the active Render Texture into it.
        Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
        image.Apply();

        // Replace the original active Render Texture.
        RenderTexture.active = currentRT;
        return image;
    }
}
