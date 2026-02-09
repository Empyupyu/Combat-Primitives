using Cysharp.Threading.Tasks;

public class UnitsController
{
    private readonly GameData _gameData;

    public UnitsController(GameData gameData) 
    {
        _gameData = gameData;
    }

    public void SetAllUnitsState(UnitState state)
    { 
        foreach (var kvp in _gameData.CurrentUnitsInBattle) 
        {
            foreach (var unit in kvp.Value)
            { 
                unit.UnitStateMachineMono.SwitchState(state).Forget();
            } 
        } 
    }
}