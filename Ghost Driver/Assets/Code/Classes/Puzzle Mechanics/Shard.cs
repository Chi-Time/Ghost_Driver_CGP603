using UnityEngine;

public class Shard : MonoBehaviour
{
    public ShardType Type { get { return _Type; } }
    public bool ShouldHide { get { return _ShouldHide; } }

    [Tooltip ("Should the mother shard hide or show it's children upon colletion?")]
    [SerializeField] private bool _ShouldHide = false;
    [Tooltip ("What shard family does this object belong to?")]
    [SerializeField] private ShardType _Type = ShardType.Alpha;
}
