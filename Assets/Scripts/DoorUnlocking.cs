using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorUnlocking : MonoBehaviour
{
    private bool m_isLocked = true;
    [SerializeField] private bool m_isLeft = true;
    private Rigidbody m_doorRb;
    private Animator m_doorAnimator;
    [SerializeField] AudioSource m_doorAudio;
    [SerializeField] GameObject[] m_handles;
    // Start is called before the first frame update
    private void Awake()
    {
        m_doorRb = GetComponent<Rigidbody>();
        m_doorAnimator = GetComponent<Animator>();

        var openTime = m_doorAudio.clip.length;
        
        m_doorAnimator.SetFloat("f_OpeningTime", 1 / openTime);
        m_doorAnimator.SetBool("b_IsClosed", true);
       
        m_doorAnimator.SetBool("b_isLeft", m_isLeft);
    }
    public void UnlockDoor()
    {
        
        if (m_isLocked)
        {
            m_isLocked = false;
            m_doorAnimator.enabled = true;
            m_doorRb.constraints -= RigidbodyConstraints.FreezeRotationY;

            m_doorAnimator.SetTrigger("t_OpenDoor");
            StartCoroutine(GivePlayerControl());
            m_doorAudio.Play();
        }
    }
    private IEnumerator GivePlayerControl()
    {
        while(!m_doorAnimator.GetCurrentAnimatorStateInfo(0).IsName("OpenedState"))
        {
            
            yield return null;
        }

        m_doorAnimator.SetBool("b_IsClosed", false);
        foreach (var handle in m_handles)
        {
            handle.GetComponent<XRGrabInteractable>().enabled = true;
        }
        m_doorAnimator.enabled = false;
    }
}
