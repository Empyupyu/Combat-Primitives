using UnityEngine;

public class CircleSpawnStrategy : UnitSpawnStrategy
{
    private readonly float _radius = 1.2f;

    public Vector3 CalculateSpawnPosition(Vector3 startPoint)
    {
        Vector2 circle = Random.insideUnitSphere * _radius;
        return new Vector3(startPoint.x + circle.x, startPoint.y, startPoint.z + circle.y);
    }
}

