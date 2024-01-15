using UnityEngine.Events;
public interface IMoveLinear
{
    public UnityEvent<float,float> OnPositionChangeBegin { get; }
    public UnityEvent OnPositionChangeEnd { get;}
    
}
