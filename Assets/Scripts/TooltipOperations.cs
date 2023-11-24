using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipOperations : MonoBehaviour
{
    [SerializeField] private Canvas m_TooltipCanvas;
    [SerializeField] private TextMeshProUGUI m_TooltipText;
    private ShowMessageFromList m_ShowMessageFromList;

    private void Awake()
    {
        m_ShowMessageFromList= m_TooltipText.GetComponent<ShowMessageFromList>();
    }

    private void ShowTooltipCanvas()
    {
        m_TooltipCanvas.gameObject.SetActive(true);
    }
    private void HideTooltipCanvas()
    {
        m_TooltipCanvas.gameObject.SetActive(false);
    }


    private void ShowTooltipStage(int stage)
    {
        m_ShowMessageFromList.ShowMessageAtIndex(stage);
    }

}
