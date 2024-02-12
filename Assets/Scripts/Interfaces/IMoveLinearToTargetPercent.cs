using UnityEngine.Events;
public interface IMoveLinearToTargetPercent
{
    public UnityEvent<float> OnPositionChangeToTargetPercentBegin { get; }
    public UnityEvent OnPositionChangeEnd { get;}
    
}
