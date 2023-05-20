using UnityEngine;
using UnityEngine.Events;

public abstract class SpellManager : MonoBehaviour
{
    public abstract void TryStartCast();
    public abstract void FinishCast();
}
