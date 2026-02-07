public class UnitStatsSetup
{
    public ShapeType ShapeType { get; }
    public SizeType SizeType { get; }
    public ColorType ColorType { get; }

    public UnitStatsSetup(ShapeType shapeType, SizeType sizeType, ColorType colorType)
    {
        ShapeType = shapeType;
        SizeType = sizeType;
        ColorType = colorType;
    }
}

