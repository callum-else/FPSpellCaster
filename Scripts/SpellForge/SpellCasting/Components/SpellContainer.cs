using System.Collections.Generic;
using UnityEngine;

public class SpellContainer : MonoBehaviour
{
    private List<SpellManager> _spells = new List<SpellManager>();
    private SpellManager _selectedSpellPrimary;
    private SpellManager _selectedSpellSecondary;

    public void AddSpell(SpellManager spell)
    {
        _spells.Add(spell);

        if (_selectedSpellPrimary == null)
            _selectedSpellPrimary = spell;
        else if (_selectedSpellSecondary == null)
            _selectedSpellSecondary = spell;
    }

    public void StartInputPrimary() => _selectedSpellPrimary.TryStartCast();
    public void ReleaseInputPrimary() => _selectedSpellPrimary.FinishCast();

    public void StartInputSecondary() => _selectedSpellSecondary.TryStartCast();
    public void ReleaseInputSecondary() => _selectedSpellSecondary.FinishCast();
}