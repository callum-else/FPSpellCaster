public class HandRecoilEvent : AggregatedUnityEvent<HandRecoilEventInfo> { }

public class HandRecoilEventInfo
{
    public RecoilType RecoilType { get; set; }
    public float Duration { get; set; }
    public float Strength { get; set; }
    public bool ResetOnCompleted { get; set; } = false;
    public bool Kill { get; set; }
}

public enum RecoilType
{
    Move,
    Punch,
}
