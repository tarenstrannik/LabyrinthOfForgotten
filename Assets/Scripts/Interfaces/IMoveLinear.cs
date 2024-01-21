using UnityEngine.Events;
public interface IMoveLinear
{
    public UnityEvent<float> OnPositionChangeBegin { get; }
    public UnityEvent OnPositionChangeEnd { get;}
    
}
