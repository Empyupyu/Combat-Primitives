using System.Collections.Generic;
using System.Linq;

public class UnitSpawnController
{
    private UnitSpawner _spawner;
    private bool _pendingRandomize;

    public void RegisterSpawner(UnitSpawner spawner)
    {
        _spawner = spawner;

        if (_pendingRandomize)
        {
            _spawner.RandomizeUnits();
            _pendingRandomize = false;
        }
    }

    public void UnregisterSpawner()
    {
        _spawner = null;
    }

    public void Spawn(LevelConfig levelConfig, List<SpawnPoint> spawnPoints)
    {
        for (int i = 0; i < levelConfig.Teams.Count; i++)
        {
            var team = levelConfig.Teams[i];
            var spawnPoint = spawnPoints.FirstOrDefault(p => p.Team.Equals(team.Team));
            var otherSpawnPoint = spawnPoints.FirstOrDefault(p => !p.Team.Equals(team.Team));

            for (int u = 0; u < team.UnitCount; u++)
            {
                _spawner.Spawn(team.Team, spawnPoint.transform.position, otherSpawnPoint.transform.position);
            }
        }
    }

    public void RequestRandomize()
    {
        if (_spawner != null)
            _spawner.RandomizeUnits();
        else
            _pendingRandomize = true;
    }
}

