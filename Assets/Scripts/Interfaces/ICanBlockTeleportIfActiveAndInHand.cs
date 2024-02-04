using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ICanBlockTeleportIfActiveAndInHand
{
    public UnityEvent<GameObject> OnActivated { get; }
    public UnityEvent<GameObject> OnDeactivated { get; }

    public bool IsActive { get; }
}
