using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SequentialMultiSpellManager : SpellManager
{
    [SerializeField] private List<SpellEffectAnimator> _effectAnimators;

    private IEnumerator<SpellEffectAnimator> _animatorEnumerator;

    private bool _isCasting;
    private bool _isAnimatorCastable;
    private float _cooldownFinishTime;
    private float _nextSpellCastTime;

    protected List<SpellEffectAnimator> EffectAnimators 
    { 
        get => _effectAnimators;
    }

    protected abstract float CastingCooldown { get; }
    protected abstract float TimeBetweenSpells { get; }
    protected abstract IEnumerator<SpellEffectAnimator> GetEnumerator();
    protected abstract void OnFinishCast(bool hasCastables);
    protected abstract void OnStartCast();

    public override void FinishCast()
    {
        if (_isCasting)
        {
            _isCasting = false;
            StopAllCoroutines();

            if (Time.time >= _cooldownFinishTime)
                _cooldownFinishTime = Time.time + CastingCooldown;

            var playingAnimators = _effectAnimators.Where(xx => xx.IsPlaying || xx.IsCastable);

            var castables = playingAnimators.Where(xx => xx.IsCastable);
            foreach (var animator in castables)
                animator.PlayFinishedCastingAnimation();

            var cancelled = playingAnimators.Where(xx => !xx.IsCastable);
            foreach (var animator in cancelled)
                animator.PlayCancelCastingAnimation();

            OnFinishCast(castables.Any());
        }
    }

    public override void TryStartCast()
    {
        if (Time.time >= _cooldownFinishTime)
        {
            _animatorEnumerator = GetEnumerator();
            if (_animatorEnumerator.MoveNext())
            {
                _isAnimatorCastable = false;
                InitialiseAnimatorAndPlay(_animatorEnumerator.Current);
                _isCasting = true;

                OnStartCast();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isCasting && _isAnimatorCastable && Time.time >= _nextSpellCastTime)
        {
            _isAnimatorCastable = false;
            if (_animatorEnumerator.MoveNext())
                InitialiseAnimatorAndPlay(_animatorEnumerator.Current);
        }
    }

    private void InitialiseAnimatorAndPlay(SpellEffectAnimator animator)
    {
        animator.OnIsCastable.AddListener(() =>
        {
            _nextSpellCastTime = Time.time + TimeBetweenSpells;
            _isAnimatorCastable = true;
        });
        animator.PlayCastingAnimation();
    }
}
