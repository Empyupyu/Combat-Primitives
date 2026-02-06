using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSpawner
{
    private readonly UnitSpawnStrategy _unitSpawnStrategy;
    private readonly UnitView _unitPrefab;
    private readonly StatsFactory _statsFactory;
    private readonly StatsCreator _statsCreator;
    private readonly UnitModifierConfig _unitModifierConfig;
    private Dictionary<UnitTeam, List<UnitView>> _units;

    public UnitSpawner(
        UnitSpawnStrategy unitSpawnStrategy,
        UnitView unitPrefab,
        StatsFactory statsFactory,
        StatsCreator statsCreator,
        UnitModifierConfig unitModifierConfig)
    {
        _unitSpawnStrategy = unitSpawnStrategy;
        _unitPrefab = unitPrefab;
        _statsFactory = statsFactory;
        _statsCreator = statsCreator;
        _unitModifierConfig = unitModifierConfig;

        _units = new Dictionary<UnitTeam, List<UnitView>>();
    }

    public void Spawn(UnitTeam unitTeam, Vector3 startPoint, Vector3 otherTeamPoint)
    {
        UnitView unitView = CreateUnitView(startPoint, otherTeamPoint);

        if (!_units.ContainsKey(unitTeam)) 
        {
            _units[unitTeam] = new List<UnitView>(); 
        }

        _units[unitTeam].Add(unitView);

        InitializeUnit(unitTeam, unitView);
    }

    private UnitView CreateUnitView(Vector3 startPoint, Vector3 otherTeamPoint)
    {
        Vector3 spawnPoint = _unitSpawnStrategy.CalculateSpawnPosition(startPoint);
        Vector3 lookPoint = new Vector3(spawnPoint.x, otherTeamPoint.y, otherTeamPoint.z);

        UnitView unitView = GameObject.Instantiate(_unitPrefab);
        unitView.transform.position = spawnPoint;
        unitView.transform.LookAt(lookPoint);
        return unitView;
    }

    private void InitializeUnit(UnitTeam unitTeam, UnitView unitView)
    {
        UnitStatsSetup unitStatsSetup = _statsCreator.GetStatSetup();

        if (unitView.Model != null)
        {
            GameObject.Destroy(unitView.Model);
        }

        GameObject model = GameObject.Instantiate(_unitModifierConfig.Shapes.
          First(x => x.ShapeType == unitStatsSetup.ShapeType).ShapeMesh, unitView.transform);

        unitView.SetModel(model);

        IStats stats = _statsFactory.Create(unitStatsSetup.ShapeType, unitStatsSetup.SizeType, unitStatsSetup.ColorType);

        Unit unit = new Unit(unitTeam,stats);
        unitView.SetUnit(unit);
    }

    public void RandomizeUnits()
    {
        foreach (var pair in _units)
        {
            UnitTeam team = pair.Key;
            List<UnitView> views = pair.Value;

            foreach (var view in views)
            {
                InitializeUnit(team, view);
            }
        }
    }
}

