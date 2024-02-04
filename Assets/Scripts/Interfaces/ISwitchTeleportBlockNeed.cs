using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface ISwitchTeleportBlockNeed
{
    public UnityEvent OnTeleportBlock { get; }
    public UnityEvent OnTeleportUnblock { get; }

    public bool IsTeleportBlocked { get; }
}
