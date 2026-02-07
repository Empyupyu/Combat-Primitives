using System.Collections.Generic;

public class GameData
{
    public Level Level;
    public Dictionary<UnitTeam, List<UnitView>> CurrentUnitsInBattle;
    public ObjectPool<UnitView> UnitViewPool;
}