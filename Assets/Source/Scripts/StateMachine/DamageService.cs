using System;
using UnityEngine;

public class DamageService
{
    public event Action AllUnitsInTeamDeath;

    private readonly GameData _gameData;

    public DamageService(GameData gameData)
    {
        _gameData = gameData;
    }

    public void ApplyDamage(UnitView target, float damage)
    {
        target.Unit.ApplyDamage(damage);

        if (target.Unit.CurrentHealth > 0) return;

        _gameData.Units[target.Unit.UnitTeam].Remove(target);
        GameObject.Destroy(target.gameObject);

        foreach (var teams in _gameData.Units)
        {
            if (teams.Value.Count == 0)
            {
                AllUnitsInTeamDeath?.Invoke();
                return;
            }
        }
    }
}