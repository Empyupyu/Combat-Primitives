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
    private readonly BattleController _battleController;
    private readonly DamageService _damageService;
    private readonly GameData _gameData;
    private readonly ShapeViewPoolService _shapeViewPoolService;

    public UnitSpawner(
        UnitSpawnStrategy unitSpawnStrategy,
        UnitView unitPrefab,
        StatsFactory statsFactory,
        StatsCreator statsCreator,
        UnitModifierConfig unitModifierConfig,
        BattleController battleController,
        DamageService damageService,
        GameData gameData)
    {
        _unitSpawnStrategy = unitSpawnStrategy;
        _unitPrefab = unitPrefab;
        _statsFactory = statsFactory;
        _statsCreator = statsCreator;
        _unitModifierConfig = unitModifierConfig;
        _battleController = battleController;
        _damageService = damageService;
        _gameData = gameData;

        _shapeViewPoolService = new ShapeViewPoolService(_unitModifierConfig);
    }

    public void Spawn(UnitTeam unitTeam, Vector3 startPoint, Vector3 otherTeamPoint)
    {
        UnitView unitView = CreateUnitView(startPoint, otherTeamPoint);

        if (!unitView.UnitStateMachineMono.IsInitialized)
        {
            InitializeStateMachine(unitView);
        }

        AddUnit(unitTeam, unitView);

        InitializeUnit(unitTeam, unitView);

        _battleController.Register(unitView);
    }

    private void AddUnit(UnitTeam unitTeam, UnitView unitView)
    {
        _gameData.CurrentUnitsInBattle ??= new Dictionary<UnitTeam, List<UnitView>>();

        if (!_gameData.CurrentUnitsInBattle.ContainsKey(unitTeam))
        {
            _gameData.CurrentUnitsInBattle[unitTeam] = new List<UnitView>();
        }
        
        _gameData.CurrentUnitsInBattle[unitTeam].Add(unitView);
    }

    private void InitializeStateMachine(UnitView unitView)
    {
        List<IState<UnitState>> states = new List<IState<UnitState>>()
        {
            new IdleState(unitView.UnitStateMachineMono),
            new ChaseState(unitView.UnitStateMachineMono, unitView, _battleController),
            new AttackState(unitView.UnitStateMachineMono, unitView, _damageService),
        };

        StateMachine<UnitState> stateMachine = new StateMachine<UnitState>(states);

        unitView.UnitStateMachineMono.SetStateMachine(stateMachine);
    }

    private UnitView CreateUnitView(Vector3 startPoint, Vector3 otherTeamPoint)
    {
        Vector3 spawnPoint = _unitSpawnStrategy.CalculateSpawnPosition(startPoint);
        Vector3 lookPoint = new Vector3(spawnPoint.x, otherTeamPoint.y, otherTeamPoint.z);

        if(_gameData.UnitViewPool == null)
        {
            int totalUnits = _gameData.Level.LevelConfig.Teams.Sum(team => team.UnitCount);
            _gameData.UnitViewPool = new ObjectPool<UnitView>(_unitPrefab, totalUnits);
        }

        UnitView unitView = _gameData.UnitViewPool.Get();
        unitView.transform.position = spawnPoint;
        unitView.transform.LookAt(lookPoint);
        unitView.gameObject.SetActive(true);
        unitView.ResetView();
        return unitView;
    }

    private void InitializeUnit(UnitTeam unitTeam, UnitView unitView)
    {
        UnitStatsSetup unitStatsSetup = _statsCreator.GetStatSetup();

        if (unitView.Shape != null)
        {
            _shapeViewPoolService.Return(unitView.Shape);
        }

        CreateShapeView(unitView, unitStatsSetup);
        ApplyVisualChanges(unitView, unitStatsSetup);

        IStats stats = _statsFactory.Create(unitStatsSetup.ShapeType, unitStatsSetup.SizeType, unitStatsSetup.ColorType);

        Unit unit = new Unit(unitTeam, stats);
        unitView.SetUnit(unit);
    }

    private void ApplyVisualChanges(UnitView unitView, UnitStatsSetup unitStatsSetup)
    {
        var sizeMod = _unitModifierConfig.GetSize(unitStatsSetup.SizeType);
        var colorMod = _unitModifierConfig.GetColor(unitStatsSetup.ColorType);

        unitView.Shape.transform.localScale = sizeMod.OriginalSize * sizeMod.Size;
        unitView.Shape.transform.localPosition = unitView.Shape.transform.localScale / 2;
        unitView.SetColor(colorMod.Color);
    }

    private void CreateShapeView(UnitView unitView, UnitStatsSetup unitStatsSetup)
    {
        ShapeType type = unitStatsSetup.ShapeType;
        
        ShapeView shapeView = _shapeViewPoolService.Get(type);
        shapeView.transform.SetParent(unitView.transform);
        shapeView.transform.localPosition = Vector3.zero; 
        shapeView.gameObject.SetActive(true);

        unitView.SetShape(shapeView);
    }

    public void RandomizeUnits()
    {
        foreach (var pair in _gameData.CurrentUnitsInBattle)
        {
            UnitTeam team = pair.Key;
            List<UnitView> views = pair.Value;

            foreach (var view in views)
            {
                InitializeUnit(team, view);
            }
        }
    }

    public void Cleanup()
    {
        foreach (var kvp in _gameData.CurrentUnitsInBattle)
        {
            var team = kvp.Key;
            var units = kvp.Value;

            foreach (var unit in units)
            {
                _shapeViewPoolService.Return(unit.Shape);

                unit.ResetView();
                _gameData.UnitViewPool.ReturnToPool(unit);
            }
        }

        _gameData.CurrentUnitsInBattle.Clear();
    }
}