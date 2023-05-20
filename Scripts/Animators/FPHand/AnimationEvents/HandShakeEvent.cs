using UnityEngine;

public class HandShakeEvent : AggregatedUnityEvent<HandShakeEventInfo> { }

public class HandShakeEventInfo : IDOShakeEventInfo
{
    public float Duration { get; set; }
    public Vector3 Strength { get; set; }
    public int Vibrato { get; set; }
    public bool Kill { get; set; }
    public int Loops { get; set; }

    public static HandShakeEventInfo CastingShake
    {
        get => new HandShakeEventInfo()
        {
            Loops = -1,
            Duration = 1,
            Strength = Vector3.one * 0.01f,
            Vibrato = 20,
        };
    }
}