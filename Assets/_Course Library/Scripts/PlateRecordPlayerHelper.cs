using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlateRecordPlayerHelper : MonoBehaviour
{
    [SerializeField] private GameObject m_helperCanvas;
    [SerializeField] private string m_playerTag="Player";
    private SphereCollider m_collider;
    private int curStage = 0;
    [SerializeField] private float m_TooltipDistance = 1f;

    private ShowMessageFromList m_showMessageFromList;

    private void Awake()
    {
        m_collider = GetComponent<SphereCollider>();
        m_collider.radius = m_TooltipDistance;
        m_showMessageFromList = GetComponent<ShowMessageFromList>();
    }
    private void ShowTooltipCanvas(bool visible)
    {
        m_helperCanvas.SetActive(visible);
    }
    private void SetStage(int stage)
    {
        curStage = stage;
        m_showMessageFromList.ShowMessageAtIndex(curStage);
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag(m_playerTag))
        {
            HelperManager.m_Instance.SendMessage("PlayerNearRecordPlayer", new RecordPlayerNearMessage(gameObject, true, curStage), SendMessageOptions.DontRequireReceiver);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(m_playerTag))
        {
            HelperManager.m_Instance.SendMessage("PlayerNearRecordPlayer", new RecordPlayerNearMessage(gameObject, false, 0), SendMessageOptions.DontRequireReceiver);

        }
    }

}

public class RecordPlayerNearMessage
{
    public GameObject m_recordPlayer;
    public bool m_near;
    public int m_stage;
    
    public RecordPlayerNearMessage(GameObject recordPlayer, bool near, int stage)
    {
        m_recordPlayer = recordPlayer;
        m_near = near;
        m_stage = stage;
    }
}