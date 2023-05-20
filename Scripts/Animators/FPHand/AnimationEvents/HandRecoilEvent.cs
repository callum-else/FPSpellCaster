public class HandRecoilEvent : AggregatedUnityEvent<HandRecoilEventInfo> { }

public class HandRecoilEventInfo
{
    public RecoilType RecoilType { get; set; }
    public float Duration { get; set; }
    public float Strength { get; set; }
    public bool ResetOnCompleted { get; set; } = false;
    public bool Kill { get; set; }

    public static HandRecoilEventInfo CastingPunchRecoil 
    { 
        get => new HandRecoilEventInfo()
        {
            RecoilType = RecoilType.Punch,
            Strength = 0.15f,
            Duration = 0.15f,
            ResetOnCompleted = true,
        };
    }

    public static HandRecoilEventInfo CastingSmoothRecoil
    {
        get => new HandRecoilEventInfo()
        {
            RecoilType = RecoilType.Move,
            Strength = 0.1f,
            Duration = 0.15f,
        };
    }

    public static HandRecoilEventInfo CastingSmoothKill
    {
        get => new HandRecoilEventInfo()
        {
            Kill = true,
            Duration = 0.15f
        };
    }
}

public enum RecoilType
{
    Move,
    Punch,
}
