using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using static UnityEngine.Rendering.DebugUI;

public class PlateRecordPlayer : AudioPlayer
{

    private Coroutine rotationCoroutine;
    private Coroutine handleMovingWhileAudioPlayingCoroutine;
    private Coroutine handlePrepareCoroutine;
    private Coroutine afterHandlePrepareCoroutine;

    [SerializeField] private float RotationSpeed = 2f;

    [SerializeField] private GameObject recordPlate;



    [SerializeField] private Transform m_StartPlayingTransform;
    [SerializeField] private Transform m_EndPlayingTransform;
    [SerializeField] private Transform m_BaseHandleTransform;
    //==================
    private float rotationFromStartToFinish;
    private float m_angleCurToStart;
    private float m_angleCurToEnd;
    //==================
    [SerializeField] private GameObject handle;
    [SerializeField] private GameObject handleDynamicAttach;
    private float audioLengthTime;
    [SerializeField] private float prepareTime = 1f;
    [SerializeField] private float unprepareTime = 1f;
    private float curHandleTime;

    private bool isHandleReady = false;
    //=====================manual


    private Coroutine m_MoveDynamicHandleAttachOnHover=null;

    private Coroutine m_RotatePlate = null;


    private float m_clipTimePercent;

    // Start is called before the first frame update

    private void Awake()
    {
        rotationFromStartToFinish = Quaternion.Angle(m_StartPlayingTransform.rotation, m_EndPlayingTransform.rotation);
        //rotationFromStartToFinish = Quaternion.Dot(m_StartPlayingTransform.rotation, m_EndPlayingTransform.rotation);
    }

    protected override void Start()
    {
        base.Start();
    }

    

    public void AutoPlayRecord()
    {
        if (handlePrepareCoroutine != null) StopCoroutine(handlePrepareCoroutine);
        handlePrepareCoroutine = StartCoroutine(MoveHandle(handle, m_BaseHandleTransform.rotation, m_StartPlayingTransform.rotation, prepareTime));

        if (afterHandlePrepareCoroutine != null) StopCoroutine(afterHandlePrepareCoroutine);
        afterHandlePrepareCoroutine = StartCoroutine(AutoAfterHadlePrepare());
        

    }
    IEnumerator AutoAfterHadlePrepare()
    {
        while(!isHandleReady)
        {
            
            yield return null;
        }
        
        base.PlayRecord();
        if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);
        rotationCoroutine = StartCoroutine(RotatePlate());

        if (handleMovingWhileAudioPlayingCoroutine != null) StopCoroutine(handleMovingWhileAudioPlayingCoroutine);

        audioLengthTime = audioClipFromRecord.length;
        //handle.transform.rotation = Quaternion.Euler(0, startHandleYAngle, 0);

        handleMovingWhileAudioPlayingCoroutine = StartCoroutine(MoveHandle(handle, m_StartPlayingTransform.rotation, m_EndPlayingTransform.rotation, audioLengthTime));
    }
    public void AutoStopRecord()
    {
        base.StopRecord();
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }

        StopCoroutine(handleMovingWhileAudioPlayingCoroutine);
        if (afterHandlePrepareCoroutine != null) StopCoroutine(afterHandlePrepareCoroutine);
        if (handlePrepareCoroutine != null) StopCoroutine(handlePrepareCoroutine);
        
        handlePrepareCoroutine = StartCoroutine(MoveHandle(handle, handle.transform.rotation, m_BaseHandleTransform.rotation, unprepareTime));
        //handle.transform.localEulerAngles = new Vector3(0, baseHandleYAngle, 0);
    }
    public void StartRotatePlate()
    {
        if (m_RotatePlate == null) m_RotatePlate = StartCoroutine(RotatePlate());
    }
    public void StopRotatePlate()
    {
        if (m_RotatePlate != null)
        {
            StopCoroutine(m_RotatePlate);
            m_RotatePlate = null;
        }
    }
    IEnumerator RotatePlate()
    {
        while (true)
        {
            recordPlate.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
            yield return null;
        }
    }


    IEnumerator MoveHandle(GameObject handle, Quaternion startRotation, Quaternion endRotation,float moveTime)
    {
        curHandleTime = 0f;
        isHandleReady = false;
        
        var startParentRotation = Quaternion.Inverse(transform.rotation);

        while (curHandleTime < moveTime)
        {
            
            var curStartRotation = transform.rotation * startParentRotation* startRotation;
            var curEndRotation = transform.rotation * startParentRotation * endRotation;
            //Debug.Log(curEndRotation + " " + endRotation+" "+ (handle.transform.eulerAngles.y- transform.eulerAngles.y));
            handle.transform.rotation = Quaternion.Slerp(curStartRotation, curEndRotation, curHandleTime / moveTime);
           
            curHandleTime += Time.deltaTime;

            yield return null;
        }
        handle.transform.rotation = endRotation * transform.rotation * startParentRotation;
        isHandleReady = true;

    }
   
    public void MoveHandleOnPlay()
    {
        if (handleMovingWhileAudioPlayingCoroutine == null)
        {
             handleMovingWhileAudioPlayingCoroutine = StartCoroutine(MoveHandle(handleDynamicAttach, handle.transform.rotation, m_EndPlayingTransform.rotation, audioClipFromRecord.length * (1 - m_clipTimePercent)));
        }
    }
    public void StopHandle()
    {
        if (handleMovingWhileAudioPlayingCoroutine != null)
        {
            StopCoroutine(handleMovingWhileAudioPlayingCoroutine);
            handleMovingWhileAudioPlayingCoroutine = null;
        }
    }

    public void CalculateStartPlayParameters()
    {

        // m_clipTimePercent=Mathf.InverseLerp(rotationFromStartToFinish, Quaternion.Dot(m_StartPlayingTransform.rotation, handle.transform.rotation), Quaternion.Dot(m_EndPlayingTransform.rotation, handle.transform.rotation));

        //m_clipTimePercent = Mathf.InverseLerp(startHandleYAngle, finishHandleYAngle, m_localHandleEulerY); разные варианты рассчета

        CalculateAnglesToStartAndEnd();
        if (m_angleCurToEnd >= rotationFromStartToFinish && m_angleCurToEnd >= m_angleCurToStart)
        {
            handle.transform.rotation = m_StartPlayingTransform.rotation;
        }
        m_clipTimePercent = Mathf.InverseLerp(0, rotationFromStartToFinish, Quaternion.Angle(m_StartPlayingTransform.rotation, handle.transform.rotation));
        
    }
    //====================manual

    public void StartMoveDynamicSocketAttachOnHover()
    {
       if(m_MoveDynamicHandleAttachOnHover==null) m_MoveDynamicHandleAttachOnHover = StartCoroutine(C_MoveDynamicSocketAttachOnHover());
        
    }
    public void StopMoveDynamicSocketAttachOnHover()
    {

        if (m_MoveDynamicHandleAttachOnHover != null)
        {
            StopCoroutine(m_MoveDynamicHandleAttachOnHover);

            m_MoveDynamicHandleAttachOnHover = null;
        }
    }
    private void CalculateAnglesToStartAndEnd()
    {
        m_angleCurToStart = Quaternion.Angle(handle.transform.rotation, m_StartPlayingTransform.rotation);
        m_angleCurToEnd = Quaternion.Angle(handle.transform.rotation, m_EndPlayingTransform.rotation);
    }
    private IEnumerator C_MoveDynamicSocketAttachOnHover()
    {
   
        while(true)
        {
            CalculateAnglesToStartAndEnd();
            if (m_angleCurToEnd <= rotationFromStartToFinish && rotationFromStartToFinish >= m_angleCurToStart)
            {
                handleDynamicAttach.transform.rotation = handle.transform.rotation;
            }
            /*при грабе ручка "прыгает" в сторону нефизично. из за этого она выкакивает из области отслеживания (и, более того, выталкивается даже за пределы хинджа и динамик аттач не сдвигается. 
             * при этом за счет ширины коллайдера сокет инициализируется, и отображает аттач в старом необновленном месте (при этом ручка за пределами, поэтому перемещение аттача не срабатывает.
             * при отпускании все ок, физика выталкивает ручку обратно в сокет, и аттач случается где надо, но выглядит коряво
             * чтобы избежать этого сделана костыльная функция проверки после выхода из сокета, в какую сторону вышла ручка, и перемещение аттач трансформа туда
            */
            handleDynamicAttach.transform.rotation = handle.transform.rotation;
            yield return null;
        }
    }
    public void ChechWhereHandleAndMoveDynAttachTransform()
    {
        CalculateAnglesToStartAndEnd();
        if (m_angleCurToEnd >= rotationFromStartToFinish && m_angleCurToEnd>=m_angleCurToStart)
        {
            handleDynamicAttach.transform.rotation = m_StartPlayingTransform.rotation;
        }
        else if(m_angleCurToStart >= rotationFromStartToFinish && m_angleCurToEnd <= m_angleCurToStart)
        {
            handleDynamicAttach.transform.rotation = m_EndPlayingTransform.rotation;
        }
    }
    public void PlayRecordAtMoment()
    {
        
        audioClipFromRecord = recordInteractor.GetOldestInteractableSelected().transform.gameObject.GetComponent<AudioContainer>().AudioRecord;
        
        if (audioClipFromRecord != null)
        {
            audioSource.time = audioClipFromRecord.length * m_clipTimePercent< audioClipFromRecord.length ? audioClipFromRecord.length * m_clipTimePercent : audioClipFromRecord.length * m_clipTimePercent -0.001f;
            
            base.PlayRecord();
        }
    }
    public override void StopRecord()
    {
        base.StopRecord();

    }

    public void DisablePlateRecordCollider()
    {
        if (recordInteractor.GetOldestInteractableSelected() != null)
        {
            var collider = recordInteractor.GetOldestInteractableSelected().transform.gameObject.GetComponent<Collider>();
           
            if (collider != null) collider.enabled = false;
        }
    }
    public void EnablePlateRecordCollider()
    {
        if (recordInteractor.GetOldestInteractableSelected() != null)
        {
            var collider = recordInteractor.GetOldestInteractableSelected().transform.gameObject.GetComponent<Collider>();
            
            if (collider != null) collider.enabled = true;
        }

    }
    public void DisablePlateRecordSocket()
    {
        if (recordInteractor.GetOldestInteractableSelected() == null)
        {
            recordInteractor.enabled = false;
        }
    }

    public void EnablePlateRecordSocket()
    {

            recordInteractor.enabled = true;

    }
}
