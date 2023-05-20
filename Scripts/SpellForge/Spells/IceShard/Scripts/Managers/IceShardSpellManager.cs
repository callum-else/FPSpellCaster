using System.Collections.Generic;

public class IceShardSpellManager : SequentialMultiSpellManager
{
    private UnityEventAggregator _eventAggregator;

    protected override float CastingCooldown => 0.1f;
    protected override float TimeBetweenSpells => 0.15f;

    private void Awake()
    {
        _eventAggregator = UnityEventAggregator.GetInstance();
    }

    protected override IEnumerator<SpellEffectAnimator> GetEnumerator()
    {
        EffectAnimators.Reverse();
        return EffectAnimators.GetEnumerator();
    }

    protected override void OnFinishCast(bool hasCastables)
    {
        _eventAggregator.GetEvent<CameraShakeEvent>().Publish(new CameraShakeEventInfo() { Kill = true });
        _eventAggregator.GetEvent<HandShakeEvent>().Publish(new HandShakeEventInfo() { Kill = true });

        if (hasCastables)
        {
            _eventAggregator.GetEvent<CameraPunchEvent>().Publish(CameraPunchEventInfo.CastingImpactPunch);
            _eventAggregator.GetEvent<HandRecoilEvent>().Publish(HandRecoilEventInfo.CastingPunchRecoil);
        }
        else
            _eventAggregator.GetEvent<HandRecoilEvent>().Publish(HandRecoilEventInfo.CastingSmoothKill);
    }

    protected override void OnStartCast()
    {
        _eventAggregator.GetEvent<CameraShakeEvent>().Publish(CameraShakeEventInfo.CastingShake);
        _eventAggregator.GetEvent<HandShakeEvent>().Publish(HandShakeEventInfo.CastingShake);
        _eventAggregator.GetEvent<HandRecoilEvent>().Publish(HandRecoilEventInfo.CastingSmoothRecoil);
    }
}
