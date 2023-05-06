using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpellContainer))]
public class SpellInjector : MonoBehaviour
{
    [SerializeField] private SpellManager _spell;

    private void Start()
    {
        var container = GetComponent<SpellContainer>();
        container.AddSpell(_spell);
    }
}
