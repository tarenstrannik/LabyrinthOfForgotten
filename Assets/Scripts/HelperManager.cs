using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HelperManager : MonoBehaviour
{
    private static HelperManager m_instance;
    public static HelperManager m_Instance
    {
        get
        {
            if(m_instance == null)
            {
                GameObject hm = new GameObject("HelperManager");
                hm.AddComponent<HelperManager>();
            }
            return m_instance;
        }
    }
    private GameObject m_PlateRecordPlayer;
    private bool m_ManualWasFinished = false;
    [SerializeField] private float m_endTimeout = 5f;
    private int curStage = 0;


    private Coroutine m_helperCoroutine=null;

    [SerializeField] private string m_RecordTag = "PlateRecord";

    [SerializeField] private string m_RecordPlayerSocketTag = "PlateRecordPlayer";

    [SerializeField] private string m_RecordPlayerHandleTag = "PlateRecordPlayerHandle";
    [SerializeField] private string m_RecordPlayerDynamicSocketTag = "PlateRecordPlayerDynamicSocket";

    private void Awake()
    {
        m_instance = this;
    }
    // Start is called before the first frame update
    
    public void PlayerNearRecordPlayer(RecordPlayerNearMessage recordPlayerNearMessage)
    {
        if (!m_ManualWasFinished)
        {
            if (recordPlayerNearMessage.m_near)
            {
                m_PlateRecordPlayer = recordPlayerNearMessage.m_recordPlayer;
                curStage= recordPlayerNearMessage.m_stage;

                    m_PlateRecordPlayer.SendMessage("ShowTooltipCanvas", true, SendMessageOptions.DontRequireReceiver);

                    if (m_helperCoroutine == null) m_helperCoroutine = StartCoroutine(HelperStages());

            }
            else
            {
                m_PlateRecordPlayer.SendMessage("ShowTooltipCanvas", false, SendMessageOptions.DontRequireReceiver);
                if (m_helperCoroutine != null)
                {
                    StopCoroutine(m_helperCoroutine);
                    m_helperCoroutine = null;
                }
            }
        }
    }
    private void ObjectSelected(SelectEnterEventArgsWithParentInfo argsWithParent)
    {
        
        if (!m_ManualWasFinished)
        {
            if (argsWithParent.args.interactableObject.transform.gameObject.CompareTag(m_RecordTag))
            {

                if (argsWithParent.args.interactorObject.transform.gameObject.CompareTag(m_RecordPlayerSocketTag) && argsWithParent.args.interactorObject.transform.parent.gameObject == m_PlateRecordPlayer)
                {

                    curStage = 2;

                }
                else if (curStage == 0)
                {
                    curStage = 1;
                }
            }
            else if (argsWithParent.args.interactableObject.transform.gameObject.CompareTag(m_RecordPlayerHandleTag) && argsWithParent.m_originalInteractableParent.gameObject == m_PlateRecordPlayer)
            {
                if (argsWithParent.args.interactorObject.transform.gameObject.CompareTag(m_RecordPlayerDynamicSocketTag) && argsWithParent.args.interactorObject.transform.parent.gameObject== m_PlateRecordPlayer)
                {

                    curStage = 4;
                    m_ManualWasFinished = true;

                }
                else if (curStage == 2 && argsWithParent.args.interactorObject is XRBaseControllerInteractor controllerInteractor && controllerInteractor != null)
                {
                    curStage = 3;
                }
            }
        }
        
    }
    private void ObjectDeselected(SelectExitEventArgsWithParentInfo argsWithParent)
    {
        
        if (!m_ManualWasFinished)
        {
            if (argsWithParent.args.interactableObject.transform.gameObject.CompareTag(m_RecordTag))
            {
                if (curStage == 1)
                {
                    curStage = 0;
                }
                else if (argsWithParent.args.interactorObject.transform.gameObject.CompareTag(m_RecordPlayerSocketTag) && argsWithParent.args.interactorObject.transform.parent.gameObject == m_PlateRecordPlayer)
                {

                    curStage = 1;
                }

            }
            else if (argsWithParent.args.interactableObject.transform.gameObject.CompareTag(m_RecordPlayerHandleTag) && argsWithParent.m_originalInteractableParent.gameObject == m_PlateRecordPlayer)
            {
                if (curStage == 3)
                {
                    curStage = 2;
                }
            }
        }
    }
    IEnumerator HelperStages()
    {
        var prevStage = curStage;
        while(!m_ManualWasFinished)
        {
            if(curStage!= prevStage)
            {
                prevStage = curStage;
                m_PlateRecordPlayer.SendMessage("SetStage", curStage);
            }
            yield return null;
        }
        m_PlateRecordPlayer.SendMessage("SetStage", curStage);
        yield return new WaitForSeconds(m_endTimeout);
        m_PlateRecordPlayer.SendMessage("ShowTooltipCanvas", false, SendMessageOptions.DontRequireReceiver);


    }
}
