using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
public interface IReactOnSelectionByPlayer
{
    public UnityEvent<SelectEnterEventArgs> OnByPlayerSelected { get; }
    public UnityEvent<SelectExitEventArgs> OnByPlayerDeselected { get; }
}
