using UnityEngine;

public class CameraPunchEvent : AggregatedUnityEvent<CameraPunchEventInfo> { }

public class CameraPunchEventInfo : IDOPunchEventInfo
{
    public Vector3 Punch { get; set; }
    public float Duration { get; set; }
    public int Vibrato { get; set; }
    public bool Kill { get; set; }
    public int Loops { get; set; }

    public static CameraPunchEventInfo CastingImpactPunch 
    {
        get => new CameraPunchEventInfo()
        {
            Duration = 0.15f,
            Loops = 0,
            Punch = Vector3.back * 0.15f,
            Vibrato = 20,
        };
    }
}
