using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShardSpellManager : SequentialMultiSpellManager
{
    protected override float CastingCooldown => 0.1f;
    protected override float TimeBetweenSpells => 0.15f;

    protected override IEnumerator<SpellEffectAnimator> GetEnumerator()
    {
        EffectAnimators.Reverse();
        return EffectAnimators.GetEnumerator();
    }
}
