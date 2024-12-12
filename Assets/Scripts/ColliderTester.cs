using ER.Entity2D;
using UnityEngine;

public class ColliderTester:MonoBehaviour
{
    public ColliderGroupArea group;
    public Area area;

    private void Start()
    {
        group.onEnter += (other) =>
        {
            Debug.Log($"group进入: {other.name}");
        };
        group.onExit += (other) =>
        {
            Debug.Log($"group离开: {other.name}");
        };

        area.onEnter += (other) =>
        {
            Debug.Log($"area进入: {other.name}");
        };
        area.onExit += (other) =>
        {
            Debug.Log($"area离开: {other.name}");
        };
    }
}