using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using static Unity.VisualScripting.Member;

public class CreateObjectCopy : MonoBehaviour
{
    [SerializeField] private GameObject m_source;
    [SerializeField] private Transform m_target;
    [SerializeField] private float m_modelScale = 0.3f;
    // Start is called before the first frame update
    public void CreateCopy()
    {
        var curParent = m_source.transform.parent;
        m_source.transform.parent = null;

        var newCopy = Instantiate(m_source, m_target.position, m_target.rotation);

        m_source.transform.parent = curParent;

        newCopy.name = "MyDraw";
        newCopy.transform.localScale *= m_modelScale;
        newCopy.tag = "Untagged";
        foreach (Transform child in newCopy.transform)
        {
            child.tag = "Untagged";

        }
        newCopy.AddComponent<SphereCollider>();
        newCopy.GetComponent<SphereCollider>().radius = newCopy.transform.localScale.x;
        //newCopy.GetComponent<SphereCollider>().isTrigger = true;
        newCopy.AddComponent<Rigidbody>();
        newCopy.GetComponent<Rigidbody>().isKinematic = true;

        newCopy.AddComponent<XRGrabInteractable>();
        newCopy.GetComponent<XRGrabInteractable>().useDynamicAttach = true;



    }
}
