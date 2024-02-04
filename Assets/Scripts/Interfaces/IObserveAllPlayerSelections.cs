using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public interface IObserveAllPlayerSelections
{
    public UnityEvent<SelectEnterEventArgs> OnByPlayerSelection { get; }

    public UnityEvent<SelectExitEventArgs> OnByPlayerDeselection { get; }

}
