using System;

using UnityEngine;

public interface ICollisionDetector

{
    public event Action<Collision> OnCollisionEnterDetection;
    public event Action<Collision> OnCollisionExitDetection;
    public event Action<Collider> OnTriggerEnterDetection;
    public event Action<Collider> OnTriggerExitDetection;
}
