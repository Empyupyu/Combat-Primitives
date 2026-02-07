using System.Collections.Generic;

public class ShapeViewPoolService
{
    private readonly Dictionary<ShapeType, ObjectPool<ShapeView>> _pools
        = new Dictionary<ShapeType, ObjectPool<ShapeView>>();

    public ShapeViewPoolService(UnitModifierConfig config)
    {
        foreach (var shapeConfig in config.Shapes)
        {
            ShapeType type = shapeConfig.ShapeType;
            ShapeView prefab = shapeConfig.ShapeView;

            int preloadCount = 10;
            _pools[type] = new ObjectPool<ShapeView>(prefab, preloadCount);
        }
    }

    public ShapeView Get(ShapeType type)
    {
        return _pools[type].Get();
    }

    public void Return(ShapeView view)
    {
        _pools[view.ShapeType].ReturnToPool(view);
    }
}
