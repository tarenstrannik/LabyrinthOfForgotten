using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyholeStateKeyIsInsertedAndCanMove : IState
{
    public void Enter()
    {
        Debug.Log("Enter second state");
    }
    public void Process()
    {

    }
    public void Exit()
    {

    }
}
