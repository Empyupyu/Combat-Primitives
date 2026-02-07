using UnityEngine;

public class ShapeView : MonoBehaviour
{
    [field: SerializeField] public ShapeType ShapeType { get; private set; }
    [field: SerializeField] public MeshRenderer MeshRenderer { get; private set; }
}