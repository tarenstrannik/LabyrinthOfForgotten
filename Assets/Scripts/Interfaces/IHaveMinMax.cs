using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHaveMinMax
{
    public UnityEvent OnMinReachedEvent { get; }
    public UnityEvent OnMinLeftEvent { get; }
    public UnityEvent OnMaxReachedEvent { get; }
    public UnityEvent OnMaxLeftEvent { get; }
}
