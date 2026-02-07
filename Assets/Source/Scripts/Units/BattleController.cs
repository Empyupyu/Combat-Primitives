using System;
using UnityEngine;

public class BattleController
{
    public event Action OnBattleFinished;

    private readonly GameData _gameData;

    public BattleController(GameData gameData)
    {
        _gameData = gameData;
    }

    public void Register(UnitView view)
    {
        view.OnViewDestroyed += OnUnitDeath;
    }

    private void OnUnitDeath(UnitView view)
    {
        view.OnViewDestroyed -= OnUnitDeath;

        _gameData.UnitViewPool.ReturnToPool(view);
        _gameData.CurrentUnitsInBattle[view.Unit.UnitTeam].Remove(view);
        view.ResetView();

        CheckGameEnd();
    }

    public (UnitView enemy, float distance) GetClosestEnemy(UnitView requester)
    {
        UnitTeam myTeam = requester.Unit.UnitTeam;

        UnitView closest = null;
        float minDist = float.MaxValue;

        foreach (var kvp in _gameData.CurrentUnitsInBattle)
        {
            var team = kvp.Key;
            var units = kvp.Value;

            if (team == myTeam)
                continue;

            foreach (var enemyView in units)
            {
                float dist = Vector3.Distance(
                    requester.transform.position,
                    enemyView.transform.position
                );

                if (dist < minDist)
                {
                    minDist = dist;
                    closest = enemyView;
                }
            }
        }

        return (closest, minDist);
    }

    private void CheckGameEnd()
    {
        int aliveTeams = 0;

        foreach (var kvp in _gameData.CurrentUnitsInBattle)
        {
            if (kvp.Value.Count > 0)
                aliveTeams++;
        }

        if (aliveTeams <= 1)
        {
            TransitionToFinishGame();
        }
    }

    private void TransitionToFinishGame()
    {
        foreach (var kvp in _gameData.CurrentUnitsInBattle)
        {
            foreach (var view in kvp.Value)
            {
                view.OnViewDestroyed -= OnUnitDeath;
            }
        }

        OnBattleFinished?.Invoke();
    }
}
