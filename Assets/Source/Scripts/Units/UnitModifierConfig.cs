using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UnitStatsModifiers")]
public class UnitModifierConfig : ScriptableObject
{
    [field: SerializeField] public UnitBaseStatsConfig UnitBaseConfig { get; private set; }
    [field: SerializeField] public List<UnitShape> Shapes { get; private set; }
    [field: SerializeField] public List<UnitSize> Sizes { get; private set; }
    [field: SerializeField] public List<UnitColor> Colors { get; private set; }

    private Dictionary<ShapeType, ShapeStatsModifier> _shapeDict;
    private Dictionary<SizeType, SizeStatsModifier> _sizesDict;
    private Dictionary<ColorType, ColorStatsModifier> _colorsDict;

    public void Init()
    {
        _shapeDict = Shapes.ToDictionary(x => x.ShapeType, x => x.ShapeStatsModifier);
        _sizesDict = Sizes.ToDictionary(x => x.SizeType, x => x.SizeStatsModifier);
        _colorsDict = Colors.ToDictionary(x => x.ColorType, x => x.ColorStatsModifier);
    }

    public ShapeStatsModifier GetShape(ShapeType type) => _shapeDict[type];
    public SizeStatsModifier GetSize(SizeType type) => _sizesDict[type];
    public ColorStatsModifier GetColor(ColorType type) => _colorsDict[type];
}

