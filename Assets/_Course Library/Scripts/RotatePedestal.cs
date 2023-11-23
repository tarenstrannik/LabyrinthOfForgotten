using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePedestal : MonoBehaviour
{
    public void SetPedestalYRotation(float value)
    {
        transform.localEulerAngles=new Vector3(transform.localEulerAngles.x, value,transform.localEulerAngles.z);
    }
}
